﻿namespace ConsumptionNotificationSubscriptionService.Contracts.Events;

public record SubscriptionCreated(Guid CustomerId, DateTimeOffset CreatedOn);