using System;

namespace CustomerProfileService.Contracts.Queries;

public record GetCustomerProfileResponse(int CustomerId, string? Name, DateTimeOffset DateOfBirth);
