﻿using ConsumptionNotificationSubscriptionService.Contracts.v1_0;

namespace ConsumptionSubscriptionService.Contracts.v1_0.Queries;

public record GetAbnormalConsumptionSubscriptionResponse(int CustomerId, CommunicationChannel CommunicationChannel, DateTimeOffset CreatedOn);