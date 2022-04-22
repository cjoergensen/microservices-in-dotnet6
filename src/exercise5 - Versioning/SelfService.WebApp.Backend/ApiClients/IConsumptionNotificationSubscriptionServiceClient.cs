using ConsumptionNotificationSubscriptionService.Contracts;

namespace SelfService.WebApp.Backend.ApiClients
{
    public interface IConsumptionNotificationSubscriptionServiceClient
    {
        Task CreateAbnormalConsumptionSubscription(int customerId, CommunicationChannel communicationChannel);
        Task DeleteAbnormalConsumptionSubscription(int customerId);
        Task<bool> GetAbnormalConsumptionSubscriptionStatus(int customerId);
    }
}