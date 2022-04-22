using System.ComponentModel.DataAnnotations;

namespace SelfService.WebApp.Shared.Models;

public class NotificationSettings
{
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Phonenumber { get; set; }

    [Required]
    public CommunicationChannel PreferedCommunicationChannel { get; set; }
}
