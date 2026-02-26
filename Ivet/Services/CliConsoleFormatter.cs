using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace Ivet.Services
{
    public class CliConsoleFormatter : ConsoleFormatter
    {
        public CliConsoleFormatter(IOptions<ConsoleFormatterOptions> options) : base("cli") { }

        public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
        {
            var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);
            if (string.IsNullOrEmpty(message)) return;

            switch (logEntry.LogLevel)
            {
                case LogLevel.Warning:
                    textWriter.Write("\x1b[33m");
                    textWriter.Write(message);
                    textWriter.WriteLine("\x1b[0m");
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    textWriter.Write("\x1b[31m");
                    textWriter.Write(message);
                    textWriter.WriteLine("\x1b[0m");
                    break;
                default:
                    textWriter.WriteLine(message);
                    break;
            }
        }
    }
}
