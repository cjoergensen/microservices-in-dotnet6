using Microsoft.AspNetCore.Mvc;
using SelfService.WebApp.ApiGateway.ApiClients;
using SelfService.WebApp.Data;

namespace SelfService.WebApp.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : Controller
{
    private readonly ICustomerProfileServiceClient serviceClient;
    private readonly ICustomerProfileRepository repository;

    public ProfileController(ICustomerProfileServiceClient serviceClient, Data.ICustomerProfileRepository repository)
    {
        this.serviceClient = serviceClient;
        this.repository = repository;
    }

    [HttpGet]
    [Route("{id?}")]
    public IActionResult Index(int? id)
    {
        if (!id.HasValue)
            return new BadRequestResult();

        var profile = repository.Get(id.Value);
        return new ObjectResult(profile);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Shared.Models.Profile profile)
    {
        await serviceClient.UpdateProfile(profile);
        return new OkObjectResult(profile);
    }
}