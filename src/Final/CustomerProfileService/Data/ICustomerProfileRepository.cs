using CustomerProfileService.Models;

namespace CustomerProfileService.Data
{
    public interface ICustomerProfileRepository
    {
        CustomerProfile Get(int customerId);
        Task Update(int customerId, string name, string phone, string email);
    }
}