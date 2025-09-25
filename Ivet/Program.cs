using CommandLine;
using Ivet.Verbs.Model;
using Ivet.Verbs.Services;
using System.Reflection;

namespace Ivet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var types = LoadVerbs();

            Parser.Default.ParseArguments(args, types)
                .WithParsed<UpgradeOptions>(UpgradeAction.Do)
                .WithParsed<GenerateOptions>(GenerateAction.Do)
                .WithParsed<ListOptions>(ListAction.Do)
#if DEBUG
                .WithParsed<TestOptions>(TestAction.Do)
#endif
                .WithNotParsed(HandleErrors);
        }

        //load all types using Reflection
        private static Type[] LoadVerbs()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();
        }

        private static void HandleErrors(IEnumerable<Error> obj)
        {
        }
    }
}