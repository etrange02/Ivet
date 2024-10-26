using ConsoleTables;
using ExRam.Gremlinq.Core;
using Ivet.Model;
using Ivet.Services;
using Ivet.Verbs.Model;
using System.Text.Json;

namespace Ivet.Verbs.Services
{
    public class ListAction
    {
        public static void Do(ListOptions options)
        {
            var files = new List<string>();
            files.AddRange(Directory.EnumerateFiles(options.Input, "*.json", SearchOption.AllDirectories));

            var migrations = files.ConvertAll(x => new { Name = Path.GetFileNameWithoutExtension(x), Migration = JsonSerializer.Deserialize<MigrationFile>(File.ReadAllText(x)) });

            using var database = new DatabaseService(options.IpAddress, options.Port);

            var appliedMigrations = database.GremlinqClient.V<Migration>().ToArrayAsync().AsTask().GetAwaiter().GetResult();

            var allMigrations = files
                .Select(x => new {
                    Fullname = x,
                    Object = JsonSerializer.Deserialize<MigrationFile>(File.ReadAllText(x)) ?? throw new FormatException($"File {x} has bad format")
                })
                .SelectMany(x => {
                    var filename = Path.GetFileNameWithoutExtension(x.Fullname);
                    if (x.Object.Scripts?.Any() ?? false)
                        return x.Object.Scripts.Select((y, i) => new MigrationInstance { Name = $"{filename}_#{i}", Script = y.Script, Description = $"{x.Object.Description} Part #{i}", IsMulti = true });
                    if (!string.IsNullOrEmpty(x.Object.Content))
                        return new List<MigrationInstance> { new() { Name = filename, Script = x.Object.Content, Description = x.Object.Description, IsMulti = false } };
                    return new List<MigrationInstance>();
                })
                .Select(x =>
                {
                    var y = appliedMigrations.FirstOrDefault(migr => migr.MigrationName == x.Name);
                    return new { x.Name, x.Description, Date = y?.MigrationDate, x.IsMulti };
                })
                .OrderBy(x => x.Name)
                .ToList();

            var table = new ConsoleTable("Name", "Description", "Date", "Multi?");
            allMigrations.ForEach(x => table.AddRow(x.Name, x.Description, x.Date, x.IsMulti));
            table.Write();
        }
    }
}
