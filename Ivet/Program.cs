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
                .WithParsed<UpgradeOptions>(options => UpgradeAction.Do(options))
                .WithParsed<GenerateOptions>(options => GenerateAction.Do(options))
                .WithParsed<ListOptions>(options => ListAction.Do(options))
                .WithParsed<TestOptions>(options => TestAction.Do(options))
                .WithNotParsed(errors => HandleErrors(errors));
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