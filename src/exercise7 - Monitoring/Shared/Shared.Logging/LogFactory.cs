using Elasticsearch.Net;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Shared.Logging;

public static class LogFactory
{
    public static Serilog.Core.Logger BuildLogger()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("https://cgj-test.es.westeurope.azure.elastic-cloud.com:9243"))
            {
                ModifyConnectionSettings = x =>  Modify(x),
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                       EmitEventFailureHandling.WriteToFailureSink |
                                       EmitEventFailureHandling.RaiseCallback
            })
            .CreateLogger();
    }

    private static ConnectionConfiguration Modify(ConnectionConfiguration configuration)
    {
        configuration.EnableApiVersioningHeader(true);
        configuration.BasicAuthentication("", "");
        return configuration;
    }
}