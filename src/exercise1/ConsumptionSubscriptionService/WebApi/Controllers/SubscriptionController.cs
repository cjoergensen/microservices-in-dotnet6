namespace ConsumptionSubscriptionService.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionController : ControllerBase
{
    public IActionResult Create()
    {
        // Retrieve prefered communication channel from customer profile service

        

        return new AcceptedResult();
    }
}