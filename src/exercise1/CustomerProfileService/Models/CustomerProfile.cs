namespace CustomerProfileService.Models;

public class CustomerProfile
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public CustomerProfile(int CustomerId, string Name, string PhoneNumber, string Email)
    {
        this.CustomerId = CustomerId;
        this.Name = Name;
        this.PhoneNumber = PhoneNumber;
        this.Email = Email;
    }
}