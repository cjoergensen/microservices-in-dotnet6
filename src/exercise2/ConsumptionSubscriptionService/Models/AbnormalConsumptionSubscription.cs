using ConsumptionNotificationSubscriptionService.Contracts;

namespace ConsumptionNotificationSubscriptionService.Models
{
    public class AbnormalConsumptionSubscription
    {
        public int CustomerId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public CommunicationChannel CommunicationChannel { get; set; }

        public AbnormalConsumptionSubscription(int customerId, CommunicationChannel communicationChannel)
        {
            this.CustomerId = customerId;
            this.CommunicationChannel = communicationChannel;
        }
    }
}