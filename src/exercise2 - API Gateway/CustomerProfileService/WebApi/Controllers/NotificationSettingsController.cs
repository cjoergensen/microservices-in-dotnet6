using CustomerProfileService.Contracts.Commands;
using CustomerProfileService.Data;

namespace CustomerProfileService.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationSettingsController : ControllerBase
{
    private readonly INotificationSettingsRepository notificationSettingsRepository;

    public NotificationSettingsController(INotificationSettingsRepository notificationSettingsRepository)
    {
        this.notificationSettingsRepository = notificationSettingsRepository;
    }

    [HttpGet]
    [Route("{id?}")]
    public IActionResult Get([FromRoute] int? id)
    {
        if (!id.HasValue)
            return new BadRequestResult();

        var settings = notificationSettingsRepository.Get(id.Value);
        if (settings == null)
            return new ObjectResult(new GetNotificationSettingsResponse(id.Value, ConsumptionNotificationSubscriptionService.Contracts.CommunicationChannel.Email));

        return new ObjectResult(new GetNotificationSettingsResponse(settings.CustomerId, settings.PreferedCommunicationChannel));
    }

    [HttpPut]
    public IActionResult Update([FromBody] UpdateNotificationSettings updateCommand)
    {
        if (updateCommand == null)
            return new BadRequestResult();

        var settings = notificationSettingsRepository.Get(updateCommand.CustomerId);
        if (settings == null)
            return new NotFoundResult();

        notificationSettingsRepository.Update(updateCommand.CustomerId, updateCommand.PreferedCommunicationChannel);
        return new NoContentResult();
    }
}