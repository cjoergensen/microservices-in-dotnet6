// See https://aka.ms/new-console-template for more information
using MeterDataManagement.SmartMeter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var hostBuilder = new HostBuilder()
  .ConfigureServices(services =>
     services.AddHostedService<MeterReadingService>());

await hostBuilder.RunConsoleAsync();
