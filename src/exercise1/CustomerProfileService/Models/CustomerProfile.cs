namespace CustomerProfileService.Models;

public class CustomerProfile
{
    public int CustomerId { get; }
    public string Name { get; }
    public string PhoneNumber { get;  }
    public string Email { get; }

    public CustomerProfile(int CustomerId, string Name, string PhoneNumber, string Email)
    {
        this.CustomerId = CustomerId;
        this.Name = Name;
        this.PhoneNumber = PhoneNumber;
        this.Email = Email;
    }
}