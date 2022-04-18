namespace CustomerProfileService.Contracts.Commands;

public record UpdateProfile(int CustomerId, string Name, string PhoneNumber, string Email);