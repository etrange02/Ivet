using Ivet.Model;
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
            var directory = string.IsNullOrEmpty(options.Directory) ? Directory.GetCurrentDirectory() : options.Directory;
            var librarySchema = librarySchemaLoader.Load(directory);
            metaSchema.Target = LibraryToSchemaConverter.Convert(librarySchema);

            var deltaSchemaMaker = new DeltaSchemaMakerService();
            metaSchema.Difference = deltaSchemaMaker.Difference(metaSchema.Source, metaSchema.Target);

            var builder = new MigrationBuilder(metaSchema.Difference);

            var outputDirectory = string.IsNullOrEmpty(options.OutputDirectory) ? Directory.GetCurrentDirectory() : options.OutputDirectory;
            if (!string.IsNullOrEmpty(options.SprintNo))
                outputDirectory = Path.Combine(options.OutputDirectory, options.SprintNo);
            Directory.CreateDirectory(outputDirectory);

            var jsonSerializerOption = new JsonSerializerOptions {  WriteIndented = true };

            if (options.OneScriptPerFile)
            {
                var migrations = builder.BuildFileContents();

                foreach (var (migration, i) in migrations.Select((v, i) => (v, i)))
                {
                    var fileName = Path.Combine(options.OutputDirectory, $"Migration_{DateTime.Now:yyyyMMddHHmm}_{i:D3}.json");
                    File.WriteAllText(fileName, JsonSerializer.Serialize(migration, jsonSerializerOption));
                }

                return;
            }
            else
            {
                var migration = builder.BuildFileContent();

                var fileName = Path.Combine(outputDirectory, $"Migration_{DateTime.Now:yyyyMMddHHmm}.json");
                File.WriteAllText(fileName, JsonSerializer.Serialize(migration, jsonSerializerOption));
            }
        }
    }
}
