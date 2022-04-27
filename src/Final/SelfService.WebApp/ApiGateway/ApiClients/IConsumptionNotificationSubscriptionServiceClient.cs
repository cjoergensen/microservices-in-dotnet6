using ConsumptionNotificationSubscriptionService.Contracts.v1_0;

namespace SelfService.WebApp.ApiGateway.ApiClients
{
    public interface IConsumptionNotificationSubscriptionServiceClient
    {
        Task CreateAbnormalConsumptionSubscription(int customerId, CommunicationChannel communicationChannel);
        Task DeleteAbnormalConsumptionSubscription(int customerId);
        Task<bool> GetAbnormalConsumptionSubscriptionStatus(int customerId);
    }
}