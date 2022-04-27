using ConsumptionNotificationSubscriptionService.Contracts.v1_0;
using NServiceBus;

namespace CustomerProfileService.Contracts.v1_0.Commands;

public record UpdateNotificationSettings(int CustomerId, CommunicationChannel PreferedCommunicationChannel) : ICommand;
