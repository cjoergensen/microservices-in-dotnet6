using ConsumptionNotificationSubscriptionService.Contracts.v1_0;

namespace ConsumptionNotificationSubscriptionService.Models
{
    public class AbnormalConsumptionSubscription
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public CommunicationChannel CommunicationChannel { get; set; }

        public AbnormalConsumptionSubscription()
        {

        }

        public AbnormalConsumptionSubscription(int customerId, CommunicationChannel communicationChannel)
        {
            this.CustomerId = customerId;
            this.CommunicationChannel = communicationChannel;
        }
    }
}