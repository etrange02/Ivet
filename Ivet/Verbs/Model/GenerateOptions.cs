using CommandLine;

namespace Ivet.Verbs.Model
{
    [Verb("generate", HelpText = "Create a migration file")]
    public class GenerateOptions
    {
        [Option("ip", HelpText = "Server IP", Default = "localhost")]
        public string IpAddress { get; set; }

        [Option("port", HelpText = "Server Port", Default = 8182)]
        public int Port { get; set; }

        [Option("dir", HelpText = "Directory where to find dlls", Required = true)]
        public string Directory { get; set; } = string.Empty;

        [Option("output", HelpText = "Directory where to put migration file", Required = true)]
        public string OutputDirectory { get; set; } = string.Empty;
    }
}
