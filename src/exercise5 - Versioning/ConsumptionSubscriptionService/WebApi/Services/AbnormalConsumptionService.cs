using MeterReadingService.Contracts.Events.v1_0;
using NServiceBus;

namespace ConsumptionNotificationSubscriptionService.WebApi.Services;

public class AbnormalConsumptionService : IHandleMessages<AbnormalConsumptionDetected>
{
    public Task Handle(AbnormalConsumptionDetected @event, IMessageHandlerContext context)
    {
        Console.WriteLine($"Abnormal Consumption Detected: {@event}");
        return Task.CompletedTask;
    }
}