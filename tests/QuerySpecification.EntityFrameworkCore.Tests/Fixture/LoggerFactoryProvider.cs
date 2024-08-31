using Microsoft.Extensions.Logging;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Tests.Fixture;

public class LoggerFactoryProvider
{
    public static readonly ILoggerFactory LoggerFactoryInstance = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
    {
        builder.AddFilter("QuerySpecification", LogLevel.Debug);
        builder.AddConsole();
    });
}
