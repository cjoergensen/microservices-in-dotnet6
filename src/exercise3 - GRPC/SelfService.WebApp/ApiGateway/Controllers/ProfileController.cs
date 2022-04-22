using Microsoft.AspNetCore.Mvc;
using SelfService.WebApp.ApiGateway.ApiClients;

namespace SelfService.WebApp.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : Controller
{
    private readonly ICustomerProfileServiceClient serviceClient;

    public ProfileController(ICustomerProfileServiceClient serviceClient)
    {
        this.serviceClient = serviceClient;
    }

    [HttpGet]
    [Route("{id?}")]
    public async Task<IActionResult> Index(int? id)
    {
        if (!id.HasValue)
            return new BadRequestResult();

        var profile = await serviceClient.GetProfile(id.Value);
        return new ObjectResult(profile);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Shared.Models.Profile profile)
    {
        await serviceClient.UpdateProfile(profile);
        return new OkObjectResult(profile);
    }
}