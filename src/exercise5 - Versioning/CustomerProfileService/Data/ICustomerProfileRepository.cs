using CustomerProfileService.Models;

namespace CustomerProfileService.Data
{
    public interface ICustomerProfileRepository
    {
        CustomerProfile Get(int customerId);
        void Update(int customerId, string name, string phone, string email);
    }
}