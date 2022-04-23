using CustomerProfileService.Contracts.v1_0.Commands;
using CustomerProfileService.Data;

namespace CustomerProfileService.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationSettingsController : ControllerBase
{
    private readonly ILogger<NotificationSettingsController> logger;
    private readonly INotificationSettingsRepository notificationSettingsRepository;

    public NotificationSettingsController(ILogger<NotificationSettingsController> logger, INotificationSettingsRepository notificationSettingsRepository)
    {
        this.logger = logger;
        this.notificationSettingsRepository = notificationSettingsRepository;
    }

    [HttpGet]
    [Route("{id?}")]
    public IActionResult Get([FromRoute] int? id)
    {
        if (!id.HasValue)
            return new BadRequestResult();

        var settings = notificationSettingsRepository.Get(id.Value);
        if(settings == null)
            return new ObjectResult(new GetNotificationSettingsResponse(id.Value, ConsumptionNotificationSubscriptionService.Contracts.v1_0.CommunicationChannel.Email));

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