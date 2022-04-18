using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using SelfService.WebApp.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddHttpClient<SelfService.WebApp.Client.Api.ProfileService>(client => 
    client.BaseAddress = new Uri("https://localhost:7115"));

builder.Services.AddHttpClient<SelfService.WebApp.Client.Api.NotificationService>(client =>
    client.BaseAddress = new Uri("https://localhost:7115"));

await builder.Build().RunAsync();