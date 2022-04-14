using CustomerProfileService.Data;

namespace WebApi.Controllers;

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
            return new NotFoundResult();

        return new ObjectResult(new GetNotificationSettingsResponse(settings.CustomerId, settings.PhoneNumber, 
            settings.Email, CustomerProfileService.Contracts.PreferedCommunicationChannel.Phonenumber));
    }
}