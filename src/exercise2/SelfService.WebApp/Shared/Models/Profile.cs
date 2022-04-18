using System.ComponentModel.DataAnnotations;

namespace SelfService.WebApp.Shared.Models;

public class Profile
{
    public int CustomerId { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public DateTimeOffset DateOfBirth { get; set; }

    public NotificationSettings NotificationSettings { get; set; }
}
