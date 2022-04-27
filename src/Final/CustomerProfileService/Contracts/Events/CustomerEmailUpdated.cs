namespace CustomerProfileService.Contracts.v1_0.Events;

public record CustomerEmailUpdated(int CustomerId, string NewEmail, DateTimeOffset OccuredOn) : BaseEvent(OccuredOn);
