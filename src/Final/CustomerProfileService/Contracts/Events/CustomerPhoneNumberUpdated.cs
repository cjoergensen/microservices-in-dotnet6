namespace CustomerProfileService.Contracts.v1_0.Events;

public record CustomerPhoneNumberUpdated(int CustomerId, string NewPhoneNumber, DateTimeOffset OccuredOn) : BaseEvent(OccuredOn);
