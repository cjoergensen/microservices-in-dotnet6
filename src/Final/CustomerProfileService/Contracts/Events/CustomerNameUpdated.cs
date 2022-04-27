namespace CustomerProfileService.Contracts.v1_0.Events;

public record CustomerNameUpdated(int CustomerId, string NewName, DateTimeOffset OccuredOn) : BaseEvent(OccuredOn);