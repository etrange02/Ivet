﻿using Ivet.Model;
using Ivet.Services;
using Ivet.Services.Converters;
using Ivet.Services.Loaders;
using Ivet.Verbs.Model;
using System.Text.Json;

namespace Ivet.Verbs.Services
{
    public class GenerateAction
    {
        public static void Do(GenerateOptions options)
        {
            using var database = new DatabaseService(options.IpAddress, options.Port);

            var metaSchema = new TransitiveSchema();

            var databaseSchemaLoader = new DatabaseSchemaLoaderService(database);
            var databaseSchema = databaseSchemaLoader.Load();
            metaSchema.Source = DatabaseToSchemaConverter.Convert(databaseSchema);

            var librarySchemaLoader = new LibraySchemaLoaderService();
            var librarySchema = librarySchemaLoader.Load(options.Directory);
            metaSchema.Target = LibraryToSchemaConverter.Convert(librarySchema);

            var deltaSchemaMaker = new DeltaSchemaMakerService();
            metaSchema.Difference = deltaSchemaMaker.Difference(metaSchema.Source, metaSchema.Target);

            var builder = new MigrationBuilder();
            builder.MetaSchema = metaSchema.Difference;

            var migrations = builder.BuildFileContent();

            Directory.CreateDirectory(options.OutputDirectory);

            foreach (var (migration, i) in migrations.Select((v, i) => (v, i)))
            {
                var fileName = Path.Combine(options.OutputDirectory, $"Migration_{DateTime.Now.ToString("yyyyMMddHHmm")}_{i.ToString("D3")}.json");
                File.WriteAllText(fileName, JsonSerializer.Serialize(migration, new JsonSerializerOptions { WriteIndented = true }));
            }
        }
    }
}
