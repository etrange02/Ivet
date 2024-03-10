using ExRam.Gremlinq.Core;
using Ivet.Model;
using Ivet.Services;
using Ivet.Verbs.Model;
using System.Globalization;
using System.Text.Json;

namespace Ivet.Verbs.Services
{
    public class UpgradeAction
    {
        public static void Do(UpgradeOptions options)
        {
            var files = new List<string>();

            if (File.Exists(options.Input))
            {
                if (options.Input.EndsWith(".json", true, CultureInfo.InvariantCulture))
                    files.Add(options.Input);
                else
                    throw new FormatException("Bad extension. Must be a json file or a directory.");
            }
            else
            {
                files.AddRange(Directory.EnumerateFiles(options.Input, "*.json"));
            }

            using var database = new DatabaseService(options.IpAddress, options.Port);

            var appliedMigrations = database.GremlinqClient.V<Migration>()
                .ToArrayAsync().Result
                .ToList();

            var migrationsToApply = files
                .ConvertAll(x => new { Fullname = x, Name = Path.GetFileNameWithoutExtension(x) })
                .Where(x => !appliedMigrations.Select(x => x.MigrationName).Contains(x.Name))
                .Select(x => x.Fullname)
                .ToList();

            migrationsToApply.ForEach(x =>
            {
                Console.WriteLine($"Loading file {x}");
                var migrationFile = JsonSerializer.Deserialize<MigrationFile>(File.ReadAllText(x)) ?? throw new FormatException($"File {x} has bad format");
                var res = database.Execute(migrationFile.Content);
                var migration = new Migration
                {
                    MigrationName = Path.GetFileNameWithoutExtension(x),
                    MigrationDate = DateTime.Now,
                };
                var migrationDB = database.GremlinqClient.AddV(migration).FirstAsync().Result;

            });
        }
    }
}
