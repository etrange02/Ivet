﻿using ExRam.Gremlinq.Core;
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
                Console.WriteLine($"Directory: {input}");
                files.AddRange(Directory.EnumerateFiles(input, "*.json", SearchOption.AllDirectories));
            }

            using var database = new DatabaseService(options.IpAddress, options.Port);

            var appliedMigrations = database.GremlinqClient.V<Migration>()
                .ToArrayAsync()
                .AsTask()
                .GetAwaiter()
                .GetResult()
                .Select(x => x.MigrationName);

            var migrationsToApply = files
                .Select(x => new  { 
                    Fullname = x,
                    Object = JsonSerializer.Deserialize<MigrationFile>(File.ReadAllText(x)) ?? throw new FormatException($"File {x} has bad format") 
                })
                .SelectMany(x => {
                    var filename = Path.GetFileNameWithoutExtension(x.Fullname);
                    if (x.Object.Scripts?.Any() ?? false)
                        return x.Object.Scripts.Select((y, i) => new MigrationInstance { Name = $"{ filename }_#{ i }", Script = y.Script, RelativePath = Path.GetRelativePath(input, x.Fullname) });
                    if (!string.IsNullOrEmpty(x.Object.Content))
                        return new List<MigrationInstance> { new() { Name = filename, Script = x.Object.Content, RelativePath = Path.GetRelativePath(input, x.Fullname) } };
                    return new List<MigrationInstance>();
                })
                .Where(x => !appliedMigrations.Contains(x.Name))
                .ToList();

            migrationsToApply.ForEach(x =>
            {
                Console.WriteLine($"Applying migration {x.Name} ({x.RelativePath})");
                var res = database.Execute(x.Script);
                var migration = new Migration
                {
                    MigrationName = x.Name,
                    MigrationDate = DateTime.Now,
                };
                var migrationDB = database.GremlinqClient.AddV(migration).FirstAsync().AsTask().GetAwaiter().GetResult();
            });
        }
    }
}
