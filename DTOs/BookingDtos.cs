using System.ComponentModel.DataAnnotations;
using MyHiep.Api.Models;

namespace MyHiep.Api.DTOs;

public class CreateBookingRequest
{
    [Required, MaxLength(120)] public string CustomerName { get; set; } = string.Empty;
    [Required, RegularExpression(@"^(0|\+84)(3|5|7|8|9)\d{8}$")] public string Phone { get; set; } = string.Empty;
    [Required, MaxLength(500)] public string Address { get; set; } = string.Empty;
    [MaxLength(300)] public string AddressNote { get; set; } = string.Empty;
    [Required] public int ServiceId { get; set; }
    public decimal? EstimatedWeight { get; set; }
    [Required] public DateTime PickupTime { get; set; }
    [MaxLength(1000)] public string Note { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    [Range(typeof(bool), "true", "true", ErrorMessage = "Consent is required.")]
    public bool Consent { get; set; }
}

public class BookingStatusRequest
{
    [Required] public BookingStatus Status { get; set; }
}

public class BookingResponse
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string AddressNote { get; set; } = string.Empty;
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal? EstimatedWeight { get; set; }
    public DateTime PickupTime { get; set; }
    public string Note { get; set; } = string.Empty;
    public double? DistanceKm { get; set; }
    public bool RequiresDistanceConfirmation { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
