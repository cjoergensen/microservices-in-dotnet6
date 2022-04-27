namespace CustomerProfileService.Contracts.v1_0.Commands;

public record UpdateProfile(int CustomerId, string Name, string PhoneNumber, string Email);