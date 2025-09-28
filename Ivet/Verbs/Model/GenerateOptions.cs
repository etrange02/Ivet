using CommandLine;

namespace Ivet.Verbs.Model
{
    [Verb("generate", HelpText = "Create a migration file")]
    public class GenerateOptions
    {
        [Option("ip", HelpText = "Server IP", Default = "localhost")]
        public string IpAddress { get; set; } = string.Empty;

        [Option("port", HelpText = "Server Port", Default = 8182)]
        public int Port { get; set; }

        [Option("dir", HelpText = "Directory where to find dlls. If not set look in current working directory")]
        public string Directory { get; set; } = string.Empty;

        [Option("output", HelpText = "Directory where to put migration file. If not set files are created in current directory")]
        public string OutputDirectory { get; set; } = string.Empty;

        [Option("sprintno", HelpText = "Sprint/Version number of this new migration", Default = null)]
        public string? SprintNo { get; set; }

        [Option("onefile", HelpText = "Create one file per script", Default = false)]
        public bool OneScriptPerFile { get; set; }
        [Option("description", HelpText = "Description to be added in generated script", Default = null)]
        public string? Description { get; set; }
    }
}
