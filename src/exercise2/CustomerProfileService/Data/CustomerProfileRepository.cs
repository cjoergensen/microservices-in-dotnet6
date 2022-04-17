using CustomerProfileService.Models;

namespace CustomerProfileService.Data;

public class CustomerProfileRepository : ICustomerProfileRepository
{
    private Dictionary<int, CustomerProfile> profiles;
    public CustomerProfileRepository()
    {
        profiles = new Dictionary<int, CustomerProfile>
        {
            [1] = new CustomerProfile(1, Name: "John Doe", PhoneNumber: "12345676", "john@test.com"),
            [2] = new CustomerProfile(1, Name: "Jane Doe", PhoneNumber: "87654321", "jane@test.com")
        };
    }

    public CustomerProfile Get(int customerId)
    {
        return profiles[customerId];
    }

    public void Update(int customerId, string name, string phone, string email)
    {
        var profile = Get(customerId);
        profile.Name = name;
        profile.PhoneNumber = phone;
        profile.Email = email;
    }
}