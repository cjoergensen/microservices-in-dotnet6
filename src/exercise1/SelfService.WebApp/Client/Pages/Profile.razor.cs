using CustomerProfileService.Contracts.Queries;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text.Json;

namespace SelfService.WebApp.Client.Pages;

public partial class Profile
{

    [Inject]
    public HttpClient? ApiClient { get; set; }

    public Models.Profile? CustomerProfile { get; set; }


    protected async override Task OnInitializedAsync()
    {
        if (ApiClient == null)
            return;


        var httpRequest = new HttpRequestMessage(HttpMethod.Get, "/profile/1");
        var httpResponse = await ApiClient.SendAsync(httpRequest);
        httpResponse.EnsureSuccessStatusCode();

        var content = await httpResponse.Content.ReadAsStringAsync();
        CustomerProfile = JsonSerializer.Deserialize<Models.Profile>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


        await base.OnInitializedAsync();
    }
}
