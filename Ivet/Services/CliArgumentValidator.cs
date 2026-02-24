namespace Ivet.Services
{
    public static class CliArgumentValidator
    {
        public static int ValidatePort(int port)
        {
            if (port < 1 || port > 65535)
                throw new ArgumentOutOfRangeException(nameof(port), $"Port must be between 1 and 65535, got {port}");
            return port;
        }

        public static long? ValidateTimeout(long? timeout)
        {
            if (timeout.HasValue && timeout.Value < 1)
                throw new ArgumentOutOfRangeException(nameof(timeout), $"Timeout must be positive, got {timeout}");
            return timeout;
        }

        public static string? ValidateSprintNo(string? sprintNo)
        {
            if (string.IsNullOrWhiteSpace(sprintNo))
                return sprintNo;
            if (sprintNo.Contains("..") || Path.IsPathRooted(sprintNo))
                throw new ArgumentException($"Invalid sprint number '{sprintNo}'. Must not contain path traversal sequences.");
            return sprintNo;
        }
    }
}
