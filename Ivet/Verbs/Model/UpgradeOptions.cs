using CommandLine;

namespace Ivet.Verbs.Model
{
    [Verb("upgrade", HelpText = "Apply an upgrade")]
    public class UpgradeOptions
    {
        [Option("input", HelpText = "Apply one migration or a directory containing migrations", Required = true)]
        public string Input { get; set; } = string.Empty;

        [Option("ip", HelpText = "Server IP", Default = "localhost")]
        public string IpAddress { get; set; }

        [Option("port", HelpText = "Server Port", Default = 8182)]
        public int Port { get; set; }
    }
}
