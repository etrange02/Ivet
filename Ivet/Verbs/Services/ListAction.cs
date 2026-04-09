using ConsoleTables;
using ExRam.Gremlinq.Core;
using Ivet.Model;
using Ivet.Services;
using Ivet.Services.Comparers;
using Ivet.Verbs.Model;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;

namespace Ivet.Verbs.Services
{
    public class ListAction
    {
        public static void Do(ListOptions options, ILoggerFactory loggerFactory)
        {
            CliArgumentValidator.ValidatePort(options.Port);

            var files = new List<string>();
            var input = string.IsNullOrEmpty(options.Input) ? Directory.GetCurrentDirectory() : options.Input;

            if (File.Exists(options.Input))
            {
                if (options.Input.EndsWith(".json", true, CultureInfo.InvariantCulture))
                    files.Add(options.Input);
                else
                    throw new FormatException("Bad extension. Must be a json file or a directory.");
            }
            else
            {
                files.AddRange(Directory.EnumerateFiles(input, "*.json", SearchOption.AllDirectories));
            }

            var migrationInstances = files
                .Select(x => new {
                    Fullname = x,
                    Object = JsonSerializer.Deserialize<MigrationFile>(File.ReadAllText(x)) ?? throw new FormatException($"File {x} has bad format")
                })
                .SelectMany(x => {
                    var filename = Path.GetFileNameWithoutExtension(x.Fullname);
                    if (x.Object.Scripts?.Any() ?? false)
                        return x.Object.Scripts.Select((y, i) => new MigrationInstance { Name = $"{filename}_#{i}", Script = y.Script, Description = $"[{i}] {x.Object.Description}", IsMulti = true, RelativePath = Path.GetRelativePath(input, x.Fullname), EvaluationTimeout = y.EvaluationTimeout ?? x.Object.EvaluationTimeout });
                    if (!string.IsNullOrEmpty(x.Object.Content))
                        return new List<MigrationInstance> { new() { Name = filename, Script = x.Object.Content, Description = x.Object.Description, IsMulti = false, RelativePath = Path.GetRelativePath(input, x.Fullname), EvaluationTimeout = x.Object.EvaluationTimeout } };
                    return new List<MigrationInstance>();
                })
                .ToList();

            using var database = new DatabaseService(options.IpAddress, options.Port, options.UseSsl);

            var appliedMigrations = FetchAppliedMigrations(database, migrationInstances.Select(x => x.Name));

            var allMigrations = migrationInstances
                .Select(x =>
                {
                    appliedMigrations.TryGetValue(x.Name, out var date);
                    return new { x.Name, x.Description, Date = date, x.IsMulti, x.RelativePath, x.EvaluationTimeout };
                })
                .OrderBy(x => x.RelativePath, NaturalSortComparer.Instance)
                .ThenBy(x => x.Name, NaturalSortComparer.Instance)
                .ToList();

            var table = new ConsoleTable("Name", "Relative path", "Description", "Date", "Multi?", "Timeout");
            allMigrations.ForEach(x => table.AddRow(x.Name, x.RelativePath, x.Description, x.Date, x.IsMulti, x.EvaluationTimeout.HasValue ? $"{x.EvaluationTimeout}ms" : ""));

            Console.WriteLine($"Directory: {input}");
            Console.WriteLine();
            Console.WriteLine("Migrations:");
            table.Write();
        }

        // Pushes the candidate names as a server-side filter so JanusGraph picks the
        // composite index `Migration_PK` (scoped by indexOnly(Migration)) instead of
        // doing a full vertex iteration. Chunked at 200 to stay below the Gremlin
        // parameter limit and the WebSocket frame size.
        internal static Dictionary<string, DateTime?> FetchAppliedMigrations(DatabaseService database, IEnumerable<string> candidateNames)
        {
            const int chunkSize = 200;
            var applied = new Dictionary<string, DateTime?>();
            var distinctNames = candidateNames.Distinct().ToList();
            if (distinctNames.Count == 0) return applied;

            foreach (var chunk in distinctNames.Chunk(chunkSize))
            {
                var found = database.GremlinqClient.V<Migration>()
                    .Where(m => chunk.Contains(m.MigrationName!))
                    .ToArrayAsync()
                    .AsTask()
                    .GetAwaiter()
                    .GetResult();
                foreach (var m in found)
                {
                    if (m.MigrationName != null) applied[m.MigrationName] = m.MigrationDate;
                }
            }
            return applied;
        }
    }
}
