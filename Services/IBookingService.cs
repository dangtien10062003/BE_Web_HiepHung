using MyHiep.Api.DTOs;
using MyHiep.Api.Models;

namespace MyHiep.Api.Services;

public interface IBookingService
{
    Task<BookingResponse> CreateAsync(CreateBookingRequest request);
    Task<List<BookingResponse>> ListAsync(BookingStatus? status);
    Task<BookingResponse?> GetAsync(int id);
    Task<bool> UpdateStatusAsync(int id, BookingStatus status);
}
