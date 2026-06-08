using System.ComponentModel.DataAnnotations;

namespace MyHiep.Api.Models;

public class Role
{
    public int Id { get; set; }
    [MaxLength(80)] public string Name { get; set; } = string.Empty;
    public ICollection<User> Users { get; set; } = new List<User>();
}
