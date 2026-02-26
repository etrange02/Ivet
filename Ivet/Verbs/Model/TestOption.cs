using CommandLine;

namespace Ivet.Verbs.Model
{
    [Verb("test", HelpText = "Loads some data in database to test...")]
    public class TestOptions
    {
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
