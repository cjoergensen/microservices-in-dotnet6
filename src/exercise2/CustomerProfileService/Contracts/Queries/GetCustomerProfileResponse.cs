using System;

namespace CustomerProfileService.Contracts.Queries;

public record GetCustomerProfileResponse(int CustomerId, string Name, string PhoneNumber, string Email);
