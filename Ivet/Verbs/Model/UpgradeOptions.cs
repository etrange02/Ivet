using CommandLine;

namespace Ivet.Verbs.Model
{
    [Verb("upgrade", HelpText = "Apply an upgrade")]
    public class UpgradeOptions
    {
        [Option("input", HelpText = "Apply one migration or a directory containing migrations. If not set, look in working directory")]
        public string Input { get; set; } = string.Empty;

        [Option("ip", HelpText = "Server IP", Default = "localhost")]
        public string? IpAddress { get; set; }

        [Option("port", HelpText = "Server Port", Default = 8182)]
        public int Port { get; set; }

        [Option("ssl", HelpText = "Use SSL/TLS for JanusGraph connection", Default = false)]
        public bool UseSsl { get; set; }

        [Option('v', "verbose", HelpText = "Enable verbose logging (debug level)", Default = false)]
        public bool Verbose { get; set; }

        [Option("timeout", HelpText = "Default evaluation timeout in milliseconds for Gremlin scripts", Default = null)]
        public long? Timeout { get; set; }

        [Option("no-verify", HelpText = "Skip post-upgrade index status verification", Default = false)]
        public bool NoVerify { get; set; }
    }
}
