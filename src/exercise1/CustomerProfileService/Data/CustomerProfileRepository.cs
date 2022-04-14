using CustomerProfileService.Models;

namespace CustomerProfileService.Data;

public class CustomerProfileRepository : ICustomerProfileRepository
{

    public CustomerProfile Get(int customerId)
    {
        return new CustomerProfile(customerId, Name: "John Doe", PhoneNumber: "1234567", "test@test.com");
    }

}
