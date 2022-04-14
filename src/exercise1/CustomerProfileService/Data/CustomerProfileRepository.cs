using CustomerProfileService.Models;

namespace CustomerProfileService.Data;

public class CustomerProfileRepository : ICustomerProfileRepository
{

    public CustomerProfile Get(int customerId)
    {
        return new CustomerProfile
        {
            CustomerId = customerId,
            Name = "John Doe",
            DateOfBirth = DateTimeOffset.Now.AddYears(-30)
        };
    }

}
