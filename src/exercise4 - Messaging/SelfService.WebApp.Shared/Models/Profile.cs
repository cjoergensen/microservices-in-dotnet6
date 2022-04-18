using ConsumptionNotificationSubscriptionService.Contracts;
using System.ComponentModel.DataAnnotations;

namespace SelfService.WebApp.Shared.Models;

public class Profile
{
    public int CustomerId { get; set; }

    [Required]
    public string Name { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Email { get; set; }
    public CommunicationChannel PreferedCommunicationChannel { get; set; }

}
