using CustomerProfileService.Contracts.v1_0.Commands;
using CustomerProfileService.Data;
using NServiceBus;

namespace CustomerProfileService.WebApi.Services;

public class NotificationSettingsService : IHandleMessages<UpdateNotificationSettings>
{
    private readonly INotificationSettingsRepository notificationSettingsRepository;

    public NotificationSettingsService(INotificationSettingsRepository notificationSettingsRepository)
    {
        this.notificationSettingsRepository = notificationSettingsRepository;
    }

    public async Task Handle(UpdateNotificationSettings updateCommand, IMessageHandlerContext context)
    {
        var settings = notificationSettingsRepository.Get(updateCommand.CustomerId);
        if (settings == null)
            return;

        await notificationSettingsRepository.Update(updateCommand.CustomerId, updateCommand.PreferedCommunicationChannel);
    }
}
