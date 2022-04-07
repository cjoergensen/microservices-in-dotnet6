namespace ConsumptionSubscriptionService.Contracts.Events;

public record SubscriptionCreated(Guid CustomerId, DateTimeOffset CreatedOn);