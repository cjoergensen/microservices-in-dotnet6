﻿using ConsumptionNotificationSubscriptionService.Contracts.v1_0;

namespace CustomerProfileService.Contracts.v1_0.Commands;

public record UpdateNotificationSettings(int CustomerId, CommunicationChannel PreferedCommunicationChannel);
