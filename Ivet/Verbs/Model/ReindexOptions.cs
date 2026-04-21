using CommandLine;

namespace Ivet.Verbs.Model
{
    [Verb("reindex", HelpText = "Force REGISTER + REINDEX on one or all stuck indices")]
    public class ReindexOptions
    {
        [Option("index", HelpText = "Target index name. If omitted, all non-ENABLED indices are reindexed.")]
        public string? IndexName { get; set; }

        [Option("await-enabled", HelpText = "Wait for ENABLED status after REINDEX (long operation)", Default = false)]
        public bool AwaitEnabled { get; set; }

        [Option("timeout-seconds", HelpText = "Per-step await timeout in seconds", Default = 300L)]
        public long TimeoutSeconds { get; set; }

        [Option("ip", HelpText = "Server IP", Default = "localhost")]
        public string IpAddress { get; set; } = string.Empty;

        [Option("port", HelpText = "Server Port", Default = 8182)]
        public int Port { get; set; }

        [Option("ssl", HelpText = "Use SSL/TLS for JanusGraph connection", Default = false)]
        public bool UseSsl { get; set; }

        [Option('v', "verbose", HelpText = "Enable verbose logging (debug level)", Default = false)]
        public bool Verbose { get; set; }
    }
}
