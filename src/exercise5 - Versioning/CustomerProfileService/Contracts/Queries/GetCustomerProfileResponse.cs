namespace CustomerProfileService.Contracts.v1_0.Queries;

public record GetCustomerProfileResponse(int CustomerId, string Name, string PhoneNumber, string Email);
