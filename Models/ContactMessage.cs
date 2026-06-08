using System.ComponentModel.DataAnnotations;

namespace MyHiep.Api.Models;

public class ContactMessage
{
    public int Id { get; set; }
    [MaxLength(120)] public string FullName { get; set; } = string.Empty;
    [MaxLength(20)] public string Phone { get; set; } = string.Empty;
    [MaxLength(200)] public string Email { get; set; } = string.Empty;
    [MaxLength(1000)] public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
