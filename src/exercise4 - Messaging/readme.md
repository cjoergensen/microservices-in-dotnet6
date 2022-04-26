
# Exercise 4 - Messaging

## Objective

In this exercise we are going to use Messaging to communicate events from the MeterReadingService to the ConsumptionSubscriptionService.

## Step 1 - Define the event

Right-Click the In the _MeterReadingService / Contracts_ project and select _Manage nuget packages_. Under the Browse tap search for _NServiceBus_ and install the latest version.

Then add a new _Events_ folder. Then add a new _AbnormalConsumptionDetected.cs_ and update it with this:

```
using NServiceBus;

namespace MeterReadingService.Contracts.Events;

public record AbnormalConsumptionDetected(int CustomerId, string MeterId, DateTimeOffset ReadingTime, Double Value) : IEvent;
```

In order for NServiceBus to reconize this message as an event use the marker interface [IEvent](https://docs.particular.net/nservicebus/messaging/messages-events-commands#identifying-messages-marker-interfaces)

With the contract in place, lets go ahead an publish an event.

## Step 2 - Publish events

First we need to add a Nuget package to NServiceBus. Right-Click the _MeterReadingService / WebApi_ project and select _Manage nuget packages_. Under the Browse tap search for _NServiceBus.Extensions.Hosting_ and install the latest version.

Then open the _MeterReadingService / WebApi / Program.cs_ file and after the ``var builder = WebApplication.CreateBuilder(args);`` add this:

```
builder.Host.UseNServiceBus(context => 
{
    var endpointConfiguration = new EndpointConfiguration("MeterReadingService");
    endpointConfiguration.UseTransport<LearningTransport>();
    return endpointConfiguration;
});
```

Also add this using statement:

```
using NServiceBus;
```

This configures the NServiceBus framework with the [Learning Transport](https://docs.particular.net/transports/learning/) and registers an [IMessagingSession](https://docs.particular.net/nservicebus/hosting/extensions-hosting?version=extensions.hosting_2#dependency-injection-integration) to the IoC Container.

Open the _MeterReadingService / WebApi / Services / PowerMeterReadingService.cs_ and update the contructor with this:

```
private readonly IMessageSession messageSession;

public PowerMeterReadingService(IMeterReadingRepository repository, IMessageSession messageSession)
{
    this.repository = repository;
    this.messageSession = messageSession;
}
```

Then update the _AbnormalPowerConsumptionDetected_ method with this:

```
public async override Task<Empty> AbnormalPowerConsumptionDetected(PowerMeterReadingMessage request, ServerCallContext context)
{
    await messageSession.Publish(new AbnormalConsumptionDetected(request.CustomerId, request.MeterId, request.ReadingTime.ToDateTimeOffset(), request.Value));
    return new Empty();
}
```

Add these using statements:

```
using MeterReadingService.Contracts.Events;
using NServiceBus;
```

Build the project.

## Step 2 - Consume events


open the _ConsumptionSubscriptionService / WebApi / Program.cs_ file and after the ``var builder = WebApplication.CreateBuilder(args);`` add this:

```
builder.Host.UseNServiceBus(context => 
{
    var endpointConfiguration = new EndpointConfiguration("MeterReadingService");
    endpointConfiguration.UseTransport<LearningTransport>();
    return endpointConfiguration;
});
```

Also add this using statement:

```
using NServiceBus;
```

In the _ConsumptionSubscriptionService / WebApi_ project add a new _Services_ folder. In that folder add the class _AbnormalConsumptionService.cs_. Update the class with this code:

```
using MeterReadingService.Contracts.Events;
using NServiceBus;

namespace ConsumptionNotificationSubscriptionService.WebApi.Services;

public class AbnormalConsumptionService : IHandleMessages<MeterReadingService.Contracts.Events.AbnormalConsumptionDetected>
{
    public Task Handle(AbnormalConsumptionDetected @event, IMessageHandlerContext context)
    {
        Console.WriteLine($"Abnormal Consumption Detected: {@event}");
        return Task.CompletedTask;
    }
}
```

Press F5 to run the solution. In the SmartMeter console app, press Enter and verify that the _ConsumptionSubsciptionService.WebApi_ console prints the event.