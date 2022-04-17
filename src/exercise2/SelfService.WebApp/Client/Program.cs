using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using SelfService.WebApp.Client;
using SelfService.WebApp.Client.Clients;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<CustomerProfileServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:8001/");
});

builder.Services.AddHttpClient<NotificationSubscriptionServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:8002/");
});

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

await builder.Build().RunAsync();