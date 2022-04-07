namespace CustomerProfileService.Models;

public record NotificationSettings(int CustomerId, string PhoneNumber, string Email, CommunicationChannel PreferedCommunicationChannel);
