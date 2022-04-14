using CustomerProfileService.Models;

namespace CustomerProfileService.Data
{
    public interface ICustomerProfileRepository
    {
        CustomerProfile Get(int customerId);
    }
}