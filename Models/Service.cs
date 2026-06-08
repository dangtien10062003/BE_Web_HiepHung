using System.ComponentModel.DataAnnotations;

namespace MyHiep.Api.Models;

public class Service
{
    public int Id { get; set; }
    [MaxLength(160)] public string Name { get; set; } = string.Empty;
    [MaxLength(600)] public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
