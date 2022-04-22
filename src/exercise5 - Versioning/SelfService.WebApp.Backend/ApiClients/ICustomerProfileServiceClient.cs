using SelfService.WebApp.Shared.Models;

namespace SelfService.WebApp.Backend.ApiClients
{
    public interface ICustomerProfileServiceClient
    {
        Task<Profile> GetProfile(int profileId);
        Task UpdateProfile(Profile profile);
    }
}