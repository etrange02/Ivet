using CommandLine;

namespace Ivet.Verbs.Model
{
    [Verb("list", HelpText = "Show all upgrades")]
    public class ListOptions
    {
        [Option("input", HelpText = "Directory containing migrations")]
        public string Input { get; set; } = string.Empty;

        [Option("ip", HelpText = "Server IP", Default = "localhost")]
        public string IpAddress { get; set; } = string.Empty;

        [Option("port", HelpText = "Server Port", Default = 8182)]
        public int Port { get; set; }

        [Option("ssl", HelpText = "Use SSL/TLS for JanusGraph connection", Default = false)]
        public bool UseSsl { get; set; }
    }
}
