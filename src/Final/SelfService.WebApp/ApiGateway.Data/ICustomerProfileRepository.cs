using ConsumptionNotificationSubscriptionService.Contracts.v1_0;
using SelfService.WebApp.Shared.Models;

namespace SelfService.WebApp.Data
{
    public interface ICustomerProfileRepository
    {
        Profile Get(int customerId);
        void UpdateCommunicationChannel(int customerId, CommunicationChannel communicationChannel);
        void UpdateEmail(int customerId, string email);
        void UpdateName(int customerId, string name);
        void UpdatePhoneNumber(int customerId, string phonenumber);
    }
}