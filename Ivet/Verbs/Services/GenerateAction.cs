using Ivet.Model;
using Ivet.Model.Meta;
using Ivet.Services;
using Ivet.Services.Comparers;
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

            metaSchema.Difference = new MetaSchema();
            metaSchema.Difference.Vertices.AddRange(metaSchema.Target.Vertices.ExceptBy(metaSchema.Source.Vertices.Select(x => x.Name), x => x.Name));
            metaSchema.Difference.Edges.AddRange(metaSchema.Target.Edges.ExceptBy(metaSchema.Source.Edges.Select(x => x.Name), x => x.Name));
            metaSchema.Difference.Properties.AddRange(metaSchema.Target.Properties.ExceptBy(metaSchema.Source.Properties.Select(x => x.Name), x => x.Name));
            metaSchema.Difference.Connections.AddRange(metaSchema.Target.Connections.Except(metaSchema.Source.Connections, new ConnectionComparer()));
            metaSchema.Difference.VertexPropertyBindings.AddRange(metaSchema.Target.VertexPropertyBindings.Except(metaSchema.Source.VertexPropertyBindings, new PropertyBindingComparer()));
            metaSchema.Difference.EdgePropertyBindings.AddRange(metaSchema.Target.EdgePropertyBindings.Except(metaSchema.Source.EdgePropertyBindings, new PropertyBindingComparer()));
            metaSchema.Difference.CompositeIndexes.AddRange(metaSchema.Target.CompositeIndexes.Except(metaSchema.Source.CompositeIndexes, new CompositeIndexComparer()));
            metaSchema.Difference.MixedIndexes.AddRange(metaSchema.Target.MixedIndexes.Except(metaSchema.Source.MixedIndexes, new MixedIndexComparer()));
            metaSchema.Difference.IndexBindings.AddRange(metaSchema.Target.IndexBindings.Except(metaSchema.Source.IndexBindings, new IndexBindingComparer()));

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
