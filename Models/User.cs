using System.ComponentModel.DataAnnotations;

namespace MyHiep.Api.Models;

public class User
{
    public int Id { get; set; }
    [MaxLength(80)] public string UserName { get; set; } = string.Empty;
    [MaxLength(200)] public string PasswordHash { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public Role? Role { get; set; }
    public bool IsActive { get; set; } = true;
}
