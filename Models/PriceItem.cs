using System.ComponentModel.DataAnnotations;

namespace MyHiep.Api.Models;

public class PriceItem
{
    public int Id { get; set; }
    [MaxLength(160)] public string Name { get; set; } = string.Empty;
    [MaxLength(80)] public string PriceText { get; set; } = string.Empty;
    [MaxLength(300)] public string Note { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
