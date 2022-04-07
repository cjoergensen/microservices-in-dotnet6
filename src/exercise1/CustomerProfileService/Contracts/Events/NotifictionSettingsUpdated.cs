namespace CustomerProfileService.Contracts.Events;

public record NotifictionSettingsUpdated(Guid CustomerId, string PhoneNumber, string Email, PreferedCommunicationChannel PreferedCommunicationChannel);