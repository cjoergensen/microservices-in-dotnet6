﻿namespace CustomerProfileService.Contracts.Commands;

public record UpdateNotificationSettings(string PhoneNumber, string Email, PreferedCommunicationChannel PreferedCommunicationChannel);
