using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Shared.Logging;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseLogging(this IHostBuilder builder, string serviceName)
    {
        SelfLog.Enable(Console.Error);

        return builder.UseSerilog(
            new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Grpc", LogEventLevel.Fatal)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://host.docker.internal:9200"))
            {
                TypeName = null,
                AutoRegisterTemplate = true,
                IndexFormat = serviceName + "-{0:yyyy-MM-dd}",
            })
            .CreateLogger());

        
    }
}