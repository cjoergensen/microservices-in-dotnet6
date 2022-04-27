using ConsumptionNotificationSubscriptionService.Contracts.v1_0;

namespace CustomerProfileService.Contracts.v1_0.Events;

public record PreferedCommunicationChannelChanged(int CustomerId, CommunicationChannel NewCommunicationChannel, DateTimeOffset OccuredOn ): BaseEvent(OccuredOn);