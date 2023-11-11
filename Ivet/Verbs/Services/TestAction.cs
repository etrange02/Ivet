using Ivet.Services;
using Ivet.Verbs.Model;

namespace Ivet.Verbs.Services
{
    public class TestAction
    {
        public static void Do(TestOptions options)
        {
            using var database = new DatabaseService(options.IpAddress, options.Port);
            database.GenerateData();
        }
    }
}
