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
            files.AddRange(Directory.EnumerateFiles(options.Input));

            var migrations = files.ConvertAll(x => new { Name = Path.GetFileNameWithoutExtension(x), Migration = JsonSerializer.Deserialize<MigrationFile>(File.ReadAllText(x)) });

            using var database = new DatabaseService(options.IpAddress, options.Port);

            var appliedMigrations = database.GremlinqClient.V<Migration>().ToArrayAsync().Result.ToList();

            var allMigrations = migrations.ConvertAll(x =>
            {
                var y = appliedMigrations.FirstOrDefault(migr => migr.MigrationName == x.Name);
                return new { Name = x.Name, Description = x.Migration?.Description, Date = y?.MigrationDate };
            }).OrderBy(x => x.Name).ToList();

            var table = new ConsoleTable("Name", "Description", "Date");
            allMigrations.ForEach(x => table.AddRow(x.Name, x.Description, x.Date));
            table.Write();
        }
    }
}
