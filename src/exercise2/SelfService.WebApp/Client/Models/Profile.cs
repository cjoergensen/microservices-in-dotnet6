using System.ComponentModel.DataAnnotations;

namespace SelfService.WebApp.Client.Models;

public class Profile
{
    public int CustomerId { get; }

    [Required]
    public string Name { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Email { get; set; }

    public Profile(int customerId, string name, string phonenumber, string email)
    {
        CustomerId = customerId;
        Name = name;
        PhoneNumber = phonenumber;
        Email = email;
    }

}
