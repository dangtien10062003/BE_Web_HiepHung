using System.ComponentModel.DataAnnotations;

namespace MyHiep.Api.Models;

public class Booking
{
    public int Id { get; set; }
    [MaxLength(120)] public string CustomerName { get; set; } = string.Empty;
    [MaxLength(20)] public string Phone { get; set; } = string.Empty;
    [MaxLength(500)] public string Address { get; set; } = string.Empty;
    [MaxLength(300)] public string AddressNote { get; set; } = string.Empty;
    public int ServiceId { get; set; }
    public Service? Service { get; set; }
    public decimal? EstimatedWeight { get; set; }
    public DateTime PickupTime { get; set; }
    [MaxLength(1000)] public string Note { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? DistanceKm { get; set; }
    public bool RequiresDistanceConfirmation { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<BookingDetail> Details { get; set; } = new List<BookingDetail>();
}
