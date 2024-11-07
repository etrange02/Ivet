using ConsoleTables;
using ExRam.Gremlinq.Core;
using Ivet.Model;
using Ivet.Services;
using Ivet.Verbs.Model;
using System.Globalization;
using System.Text.Json;

namespace Ivet.Verbs.Services
{
    public class ListAction
    {
        public static void Do(ListOptions options)
        {
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
                        return x.Object.Scripts.Select((y, i) => new MigrationInstance { Name = $"{filename}_#{i}", Script = y.Script, Description = $"{x.Object.Description} Part #{i}", IsMulti = true, RelativePath = Path.GetRelativePath(input, x.Fullname) });
                    if (!string.IsNullOrEmpty(x.Object.Content))
                        return new List<MigrationInstance> { new() { Name = filename, Script = x.Object.Content, Description = x.Object.Description, IsMulti = false, RelativePath = Path.GetRelativePath(input, x.Fullname) } };
                    return new List<MigrationInstance>();
                })
                .Select(x =>
                {
                    var y = appliedMigrations.FirstOrDefault(migr => migr.MigrationName == x.Name);
                    return new { x.Name, x.Description, Date = y?.MigrationDate, x.IsMulti, x.RelativePath };
                })
                .OrderBy(x => x.Name)
                .ToList();

            var table = new ConsoleTable("Name", "Relative path", "Description", "Date", "Multi?");
            allMigrations.ForEach(x => table.AddRow(x.Name, x.RelativePath, x.Description, x.Date, x.IsMulti));

            Console.WriteLine($"Directory: {input}");
            Console.WriteLine();
            Console.WriteLine("Migrations:");
            table.Write();
        }
    }
}
