using Microsoft.EntityFrameworkCore;
using MyHiep.Api.Data;
using MyHiep.Api.DTOs;
using MyHiep.Api.Models;

namespace MyHiep.Api.Services;

public class BookingService(AppDbContext db, IConfiguration config, IDistanceService distanceService) : IBookingService
{
    public async Task<BookingResponse> CreateAsync(CreateBookingRequest request)
    {
        var booking = new Booking
        {
            CustomerName = request.CustomerName.Trim(),
            Phone = request.Phone.Trim(),
            Address = request.Address.Trim(),
            AddressNote = request.AddressNote.Trim(),
            ServiceId = request.ServiceId,
            EstimatedWeight = request.EstimatedWeight,
            PickupTime = request.PickupTime,
            Note = request.Note.Trim(),
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Status = BookingStatus.Pending
        };

        var storeLat = config.GetValue<double>("StoreLocation:Latitude");
        var storeLng = config.GetValue<double>("StoreLocation:Longitude");
        var radiusKm = config.GetValue<double>("StoreLocation:DeliveryRadiusKm", 3);

        if (request.Latitude.HasValue && request.Longitude.HasValue)
        {
            booking.DistanceKm = Math.Round(distanceService.CalculateKm(storeLat, storeLng, request.Latitude.Value, request.Longitude.Value), 2);
            booking.RequiresDistanceConfirmation = booking.DistanceKm > radiusKm;
        }
        else
        {
            booking.RequiresDistanceConfirmation = true;
        }

        db.Bookings.Add(booking);
        await db.SaveChangesAsync();

        return (await GetAsync(booking.Id))!;
    }

    public Task<List<BookingResponse>> ListAsync(BookingStatus? status)
    {
        var query = db.Bookings.Include(x => x.Service).AsNoTracking().OrderByDescending(x => x.CreatedAt).AsQueryable();
        if (status.HasValue) query = query.Where(x => x.Status == status.Value);
        return query.Select(x => ToResponse(x)).ToListAsync();
    }

    public async Task<BookingResponse?> GetAsync(int id)
    {
        var booking = await db.Bookings.Include(x => x.Service).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return booking is null ? null : ToResponse(booking);
    }

    public async Task<bool> UpdateStatusAsync(int id, BookingStatus status)
    {
        var booking = await db.Bookings.FindAsync(id);
        if (booking is null) return false;
        booking.Status = status;
        await db.SaveChangesAsync();
        return true;
    }

    private static BookingResponse ToResponse(Booking booking) => new()
    {
        Id = booking.Id,
        CustomerName = booking.CustomerName,
        Phone = booking.Phone,
        Address = booking.Address,
        AddressNote = booking.AddressNote,
        ServiceId = booking.ServiceId,
        ServiceName = booking.Service?.Name ?? string.Empty,
        EstimatedWeight = booking.EstimatedWeight,
        PickupTime = booking.PickupTime,
        Note = booking.Note,
        DistanceKm = booking.DistanceKm,
        RequiresDistanceConfirmation = booking.RequiresDistanceConfirmation,
        Status = booking.Status,
        CreatedAt = booking.CreatedAt
    };
}
