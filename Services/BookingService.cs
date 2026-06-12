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
            Status = BookingStatus.Pending,
            CreatedAt = DateTime.Now
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

    public async Task<PagedResponse<BookingResponse>> ListAsync(BookingStatus? status, string? search, string? service, DateTime? pickupDate, string? orderDateMode, string? orderDateValue, int page, int pageSize)
    {
        page = Math.Max(1, page);
        pageSize = pageSize is 20 or 30 ? pageSize : 10;

        var query = db.Bookings.Include(x => x.Service).AsNoTracking().AsQueryable();
        if (status.HasValue) query = query.Where(x => x.Status == status.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search.Trim()}%";
            query = query.Where(x =>
                EF.Functions.Like(x.CustomerName, term) ||
                EF.Functions.Like(x.Phone, term) ||
                EF.Functions.Like(x.Address, term) ||
                EF.Functions.Like(x.AddressNote, term) ||
                EF.Functions.Like(x.Note, term) ||
                (x.Service != null && EF.Functions.Like(x.Service.Name, term)));
        }

        if (!string.IsNullOrWhiteSpace(service))
        {
            var serviceName = service.Trim();
            query = query.Where(x => x.Service != null && x.Service.Name == serviceName);
        }

        if (pickupDate.HasValue)
        {
            var start = pickupDate.Value.Date;
            var end = start.AddDays(1);
            query = query.Where(x => x.PickupTime >= start && x.PickupTime < end);
        }

        if (!string.IsNullOrWhiteSpace(orderDateMode) && !string.IsNullOrWhiteSpace(orderDateValue))
        {
            var range = ResolveOrderDateRange(orderDateMode, orderDateValue);
            if (range.HasValue)
            {
                query = query.Where(x => x.CreatedAt >= range.Value.Start && x.CreatedAt < range.Value.End);
            }
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => ToResponse(x))
            .ToListAsync();

        return new PagedResponse<BookingResponse>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
            TotalPages = total == 0 ? 1 : (int)Math.Ceiling(total / (double)pageSize)
        };
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

    private static (DateTime Start, DateTime End)? ResolveOrderDateRange(string mode, string value)
    {
        if (mode.Equals("day", StringComparison.OrdinalIgnoreCase) && DateTime.TryParse(value, out var day))
        {
            var start = day.Date;
            return (start, start.AddDays(1));
        }

        if (mode.Equals("month", StringComparison.OrdinalIgnoreCase) && DateTime.TryParse($"{value}-01", out var month))
        {
            var start = new DateTime(month.Year, month.Month, 1);
            return (start, start.AddMonths(1));
        }

        if (mode.Equals("week", StringComparison.OrdinalIgnoreCase))
        {
            var parts = value.Split("-W", StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2 && int.TryParse(parts[0], out var year) && int.TryParse(parts[1], out var week))
            {
                var jan4 = new DateTime(year, 1, 4);
                var jan4Day = jan4.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)jan4.DayOfWeek;
                var weekOneMonday = jan4.AddDays(1 - jan4Day);
                var start = weekOneMonday.AddDays((week - 1) * 7);
                return (start, start.AddDays(7));
            }
        }

        return null;
    }
}
