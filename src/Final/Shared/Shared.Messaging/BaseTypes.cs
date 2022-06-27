namespace Shared.Messaging;

public abstract record BaseMessage(string Message, string CorrelationId, DateTimeOffset OccuredOn);
public abstract record BaseEvent(string MessageId, string CorrelationId, DateTimeOffset OccuredOn) : BaseMessage(MessageId, CorrelationId, OccuredOn), IEvent;
public abstract record BaseCommand(string MessageId, string CorrelationId, DateTimeOffset OccuredOn) : BaseMessage(MessageId, CorrelationId, OccuredOn), ICommand;