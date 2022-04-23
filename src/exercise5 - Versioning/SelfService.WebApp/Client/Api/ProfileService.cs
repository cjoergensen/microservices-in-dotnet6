using SelfService.WebApp.Shared.Models;
using System.Text;
using System.Text.Json;

namespace SelfService.WebApp.Client.Api;

public class ProfileService
{
    private readonly HttpClient httpClient;

    public ProfileService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Profile> GetProfile(int profileId)
    {
        var httpResponse = await httpClient.GetAsync($"profile/{profileId}");
        httpResponse.EnsureSuccessStatusCode();

        var content = await httpResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("Unable to load 'Profile'");

        var profile = JsonSerializer.Deserialize<Profile>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (profile is null)
            throw new InvalidOperationException("Unable to load 'Profile'");

        return profile;
    }

    public async Task UpdateProfile(Profile profile)
    {
        var content = new StringContent(JsonSerializer.Serialize(profile), Encoding.UTF8, "application/json");
        var httpResponse = await httpClient.PutAsync($"profile", content);
        httpResponse.EnsureSuccessStatusCode();
    }
}
