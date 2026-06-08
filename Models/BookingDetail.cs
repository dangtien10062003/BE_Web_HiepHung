using System.ComponentModel.DataAnnotations;

namespace MyHiep.Api.Models;

public class BookingDetail
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public Booking? Booking { get; set; }
    [MaxLength(160)] public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    [MaxLength(200)] public string Note { get; set; } = string.Empty;
}
