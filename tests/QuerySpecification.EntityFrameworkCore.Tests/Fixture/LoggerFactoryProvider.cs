using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Tests.Fixture
{
    public class LoggerFactoryProvider
    {
        public static readonly ILoggerFactory LoggerFactoryInstance = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddFilter("QuerySpecification", LogLevel.Debug);
            builder.AddConsole();
        });
    }
}
