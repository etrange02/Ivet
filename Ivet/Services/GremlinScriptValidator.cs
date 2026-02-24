namespace Ivet.Services
{
    public static class GremlinScriptValidator
    {
        private static readonly string[] DangerousPatterns =
        [
            ".drop()",
            "System.exit",
            "Runtime.getRuntime",
            "ProcessBuilder",
            "Thread.sleep",
            "new File(",
            "java.io.",
            "java.net.",
        ];

        public static void Validate(string script)
        {
            foreach (var pattern in DangerousPatterns)
            {
                if (script.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException(
                        $"Migration script contains forbidden pattern '{pattern}'. Review the script manually before execution.");
            }
        }
    }
}
