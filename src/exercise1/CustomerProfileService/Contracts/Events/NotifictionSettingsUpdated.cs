using ConsumptionNotificationSubscriptionService.Contracts;

namespace CustomerProfileService.Contracts.Events;

public record NotifictionSettingsUpdated(Guid CustomerId, string PhoneNumber, string Email, CommunicationChannel PreferedCommunicationChannel);