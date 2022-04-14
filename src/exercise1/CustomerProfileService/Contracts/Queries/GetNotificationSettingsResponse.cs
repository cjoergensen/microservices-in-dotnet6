namespace CustomerProfileService.Contracts.Queries;

public record GetNotificationSettingsResponse(int CustomerId, string PhoneNumber, string Email, PreferedCommunicationChannel PreferedCommunicationChannel);