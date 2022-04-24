using Serilog;
using Serilog.Events;

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
            .CreateLogger();
    }
}