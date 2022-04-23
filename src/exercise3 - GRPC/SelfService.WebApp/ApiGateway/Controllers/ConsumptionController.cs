using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelfService.WebApp.ApiGateway.ApiClients;

namespace SelfService.WebApp.ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class ConsumptionController : ControllerBase
{
    private readonly IMeterReadingServiceClient meterReadingServiceClient;

    public ConsumptionController(IMeterReadingServiceClient meterReadingServiceClient)
    {
        this.meterReadingServiceClient = meterReadingServiceClient;
    }

    [HttpGet]
    [Route("{id?}")]
    public async Task<IActionResult> Index(int? id)
    {
        return new ObjectResult(await meterReadingServiceClient.GetMeterReadings(id.Value, DateTimeOffset.Now.AddDays(-10), DateTimeOffset.Now));
    }
}
