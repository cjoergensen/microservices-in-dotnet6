﻿using ConsumptionNotificationSubscriptionService.Contracts;

namespace CustomerProfileService.Models;

public record NotificationSettings(int CustomerId, CommunicationChannel PreferedCommunicationChannel);
