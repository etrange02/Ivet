using CommandLine;

namespace Ivet.Verbs.Model
{
    [Verb("status", HelpText = "Show JanusGraph indices and their per-key status")]
    public class StatusOptions
    {
        [Option("ip", HelpText = "Server IP", Default = "localhost")]
        public string IpAddress { get; set; } = string.Empty;

        [Option("port", HelpText = "Server Port", Default = 8182)]
        public int Port { get; set; }

        [Option("ssl", HelpText = "Use SSL/TLS for JanusGraph connection", Default = false)]
        public bool UseSsl { get; set; }

        [Option("fail-on-non-enabled", HelpText = "Exit with code 1 if any index key is not ENABLED", Default = false)]
        public bool FailOnNonEnabled { get; set; }

        [Option('v', "verbose", HelpText = "Enable verbose logging (debug level)", Default = false)]
        public bool Verbose { get; set; }
    }
}
