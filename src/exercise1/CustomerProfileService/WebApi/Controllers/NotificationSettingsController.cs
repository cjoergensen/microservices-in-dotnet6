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

    [HttpGet(Name = "GetNotificationSettings")]
    public IActionResult Get([FromBody] GetNotificationSettingsRequest request)
    {
        // Query DB
        var settings = notificationSettingsRepository.Get(request.CustomerId);
        if(settings == null)
            return new NotFoundResult();

        return new ObjectResult(new GetNotificationSettingsResponse(settings.PhoneNumber, settings.Email, CustomerProfileService.Contracts.PreferedCommunicationChannel.Phonenumber));
    }
}