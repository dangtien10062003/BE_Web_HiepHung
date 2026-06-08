using Microsoft.AspNetCore.Mvc;
using MyHiep.Api.DTOs;
using MyHiep.Api.Models;
using MyHiep.Api.Services;

namespace MyHiep.Api.Controllers;

[ApiController]
[Route("api")]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    [HttpPost("bookings")]
    public async Task<ActionResult<BookingResponse>> Create(CreateBookingRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return await bookingService.CreateAsync(request);
    }

    [HttpGet("admin/bookings")]
    public Task<List<BookingResponse>> List([FromQuery] BookingStatus? status) => bookingService.ListAsync(status);

    [HttpGet("admin/bookings/{id:int}")]
    public async Task<ActionResult<BookingResponse>> Get(int id)
    {
        var booking = await bookingService.GetAsync(id);
        if (booking is null) return NotFound();
        return booking;
    }

    [HttpPut("admin/bookings/{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, BookingStatusRequest request)
    {
        var updated = await bookingService.UpdateStatusAsync(id, request.Status);
        return updated ? NoContent() : NotFound();
    }
}
