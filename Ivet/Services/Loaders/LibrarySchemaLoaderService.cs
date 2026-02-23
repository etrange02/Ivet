using Ivet.Model;
using System.Reflection;
using Ivet.Model.Library;

namespace Ivet.Services.Loaders
{
    public class LibrarySchemaLoaderService
    {
        public Schema Load(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Directory not found: {path}");

            var fullPath = Path.GetFullPath(path);
            var schema = new Schema();

            schema.Vertices.Add(typeof(Migration));

            var files = Directory.EnumerateFiles(fullPath, "*.dll").ToList();
            Console.WriteLine($"Loading {files.Count} DLL(s) from {fullPath}");

            files.ForEach(x =>
            {
                try
                {
                    Console.WriteLine($"  Loading: {Path.GetFileName(x)}");
                    var assembly = Assembly.LoadFrom(x);
                    var graphClasses = assembly.GetTypes().Where(t => t.GetCustomAttributes<AbstractGraphItemAttribute>().Any()).ToList();
                    if (graphClasses.Any())
                    {
                        schema.Vertices.AddRange(graphClasses.Where(t => t.GetCustomAttributes<VertexAttribute>().Any()));
                        schema.Edges.AddRange(graphClasses.Where(t => t.GetCustomAttributes<EdgeAttribute>().Any()));
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                    Console.WriteLine($"  Skipped: {Path.GetFileName(x)} (not introspectable)");
                }
                catch (BadImageFormatException)
                {
                    Console.WriteLine($"  Skipped: {Path.GetFileName(x)} (not a valid .NET assembly)");
                }
            });

            return schema;
        }
    }
}
