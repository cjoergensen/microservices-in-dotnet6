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