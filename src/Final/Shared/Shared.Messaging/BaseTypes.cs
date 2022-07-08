namespace Shared.Messaging;

public abstract record BaseEvent(DateTimeOffset Timestamp) : IEvent;
public abstract record BaseCommand(DateTimeOffset Timestamp) : ICommand;