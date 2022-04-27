using NServiceBus;

namespace CustomerProfileService.Contracts.v1_0.Events;

public abstract record BaseEvent(DateTimeOffset OccuredOn) : IEvent;
