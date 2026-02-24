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
                .WithParsed<UpgradeOptions>(options => Run(() => UpgradeAction.Do(options)))
                .WithParsed<GenerateOptions>(options => Run(() => GenerateAction.Do(options)))
                .WithParsed<ListOptions>(options => Run(() => ListAction.Do(options)))
#if DEBUG
                .WithParsed<TestOptions>(options => Run(() => TestAction.Do(options)))
#endif
                .WithNotParsed(HandleErrors);
        }

        private static void Run(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                Environment.ExitCode = 1;
            }
        }

        //load all types using Reflection
        private static Type[] LoadVerbs()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();
        }

        private static void HandleErrors(IEnumerable<Error> errors)
        {
            if (errors.Any(e => e is not HelpRequestedError and not VersionRequestedError))
                Environment.ExitCode = 1;
        }
    }
}