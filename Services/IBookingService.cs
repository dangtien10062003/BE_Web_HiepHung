using MyHiep.Api.DTOs;
using MyHiep.Api.Models;

namespace MyHiep.Api.Services;

public interface IBookingService
{
    Task<BookingResponse> CreateAsync(CreateBookingRequest request);
    Task<PagedResponse<BookingResponse>> ListAsync(BookingStatus? status, string? search, string? service, DateTime? pickupDate, string? orderDateMode, string? orderDateValue, int page, int pageSize);
    Task<BookingResponse?> GetAsync(int id);
    Task<bool> UpdateStatusAsync(int id, BookingStatus status);
}
