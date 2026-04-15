using CommandLine;
using Ivet.Services;
using Ivet.Verbs.Model;
using Ivet.Verbs.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Reflection;

namespace Ivet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var types = LoadVerbs();

            CommandLine.Parser.Default.ParseArguments(args, types)
                .WithParsed<UpgradeOptions>(opts => Run(opts.Verbose, factory => UpgradeAction.Do(opts, factory)))
                .WithParsed<GenerateOptions>(opts => Run(opts.Verbose, factory => GenerateAction.Do(opts, factory)))
                .WithParsed<ListOptions>(opts => Run(opts.Verbose, factory => ListAction.Do(opts, factory)))
                .WithParsed<StatusOptions>(opts => Run(opts.Verbose, factory => StatusAction.Do(opts, factory)))
#if DEBUG
                .WithParsed<TestOptions>(opts => Run(opts.Verbose, factory => TestAction.Do(opts, factory)))
#endif
                .WithNotParsed(HandleErrors);
        }

        private static void Run(bool verbose, Action<ILoggerFactory> action)
        {
            using var loggerFactory = CreateLoggerFactory(verbose);
            try
            {
                action(loggerFactory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                Environment.ExitCode = 1;
            }
        }

        private static ILoggerFactory CreateLoggerFactory(bool verbose)
        {
            return LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(verbose ? LogLevel.Debug : LogLevel.Information);
                builder.AddConsole(options =>
                {
                    options.FormatterName = "cli";
                    options.LogToStandardErrorThreshold = LogLevel.Error;
                });
                builder.AddConsoleFormatter<CliConsoleFormatter, ConsoleFormatterOptions>();
            });
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
