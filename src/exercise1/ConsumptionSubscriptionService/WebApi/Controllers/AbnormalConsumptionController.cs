using ConsumptionNotificationSubscriptionService.Data;
using ConsumptionSubscriptionService.Contracts.Queries;

namespace ConsumptionSubscriptionService.Controllers;

[ApiController]
[Route("[controller]")]
public class AbnormalConsumptionController : ControllerBase
{
    private readonly IAbnormalConsumptionSubscriptionRepository repository;
    private readonly ILogger<AbnormalConsumptionController> logger;

    public AbnormalConsumptionController(IAbnormalConsumptionSubscriptionRepository repository, ILogger<AbnormalConsumptionController> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    public IActionResult Create()
    {
        return new AcceptedResult();
    }

    [HttpGet]
    [Route("{id?}")]
    public async Task<IActionResult> Get(int? id)
    {
        if (!id.HasValue)
            return new BadRequestResult();

        var subscription = await repository.Get(id.Value);
        if (subscription == null)
            return new NotFoundResult();

        return new ObjectResult(new GetAbnormalConsumptionSubscriptionResponse(subscription.CustomerId, subscription.CommunicationChannel, subscription.CreatedOn));
    }

    //public IActionResult Delete()
    //{

    //}
}