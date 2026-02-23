using System.Text.RegularExpressions;

namespace Ivet.Services
{
    public static class GremlinIdentifierValidator
    {
        private static readonly Regex DangerousPattern = new("['\";\\\\`{}()\\r\\n\\t]", RegexOptions.Compiled);

        public static string Validate(string identifier, string context)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException($"Empty identifier for {context}");
            if (DangerousPattern.IsMatch(identifier))
                throw new ArgumentException($"Invalid identifier '{identifier}' for {context}. Characters ' \" ; \\ ` {{ }} ( ) and newlines are not allowed.");
            return identifier;
        }
    }
}
