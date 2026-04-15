using Ivet.Model.Database;
using Ivet.Services;
using Ivet.Verbs.Model;
using Microsoft.Extensions.Logging;

namespace Ivet.Verbs.Services
{
    public class StatusAction
    {
        private const string EnabledStatus = "ENABLED";

        public static void Do(StatusOptions options, ILoggerFactory loggerFactory)
        {
            CliArgumentValidator.ValidatePort(options.Port);

            using var database = new DatabaseService(options.IpAddress, options.Port, options.UseSsl);

            var csv = database.GetIndexStatusSchema();
            var rows = new Parser().GetIndexStatusRows(csv);

            Render(rows, Console.Out);

            if (options.FailOnNonEnabled && rows.Any(r => !string.Equals(r.Status, EnabledStatus, StringComparison.OrdinalIgnoreCase)))
                Environment.ExitCode = 1;
        }

        public static void Render(IReadOnlyList<IndexStatusRow> rows, TextWriter writer)
        {
            if (rows.Count == 0)
            {
                writer.WriteLine("No indices found.");
                return;
            }

            var groups = rows
                .GroupBy(r => r.IndexName)
                .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase);

            foreach (var group in groups)
            {
                var indexType = group.First().IndexType;
                var statuses = group.Select(r => r.Status).Distinct().ToList();
                var summary = statuses.Count == 1 ? statuses[0] : $"mixed ({string.Join("/", statuses)})";

                writer.WriteLine($"{group.Key} ({indexType}, {summary})");

                var nameWidth = Math.Max(4, group.Max(r => r.PropertyName.Length));
                var typeWidth = Math.Max(8, group.Max(r => r.DataType.Length));
                var cardWidth = Math.Max(11, group.Max(r => r.Cardinality.Length));
                var statusWidth = Math.Max(6, group.Max(r => r.Status.Length));

                foreach (var row in group.OrderBy(r => r.PropertyName, StringComparer.OrdinalIgnoreCase))
                {
                    var marker = string.Equals(row.Status, EnabledStatus, StringComparison.OrdinalIgnoreCase) ? "" : "   <- stuck";
                    writer.WriteLine($"  {row.PropertyName.PadRight(nameWidth)}  {row.DataType.PadRight(typeWidth)}  {row.Cardinality.PadRight(cardWidth)}  {row.Status.PadRight(statusWidth)}{marker}");
                }
            }
        }
    }
}
