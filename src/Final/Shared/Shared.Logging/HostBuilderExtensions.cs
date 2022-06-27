using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Shared.Logging;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseLogging(this IHostBuilder builder)
    {
        return builder.UseSerilog(
            new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Grpc", LogEventLevel.Fatal)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger());
    }
}