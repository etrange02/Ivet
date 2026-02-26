using Ivet.Services;
using Ivet.Verbs.Model;
using Microsoft.Extensions.Logging;

namespace Ivet.Verbs.Services
{
    public class TestAction
    {
        public static void Do(TestOptions options, ILoggerFactory loggerFactory)
        {
            using var database = new DatabaseService(options.IpAddress, options.Port, options.UseSsl);
            database.GenerateData();
        }
    }
}
