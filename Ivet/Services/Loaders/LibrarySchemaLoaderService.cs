using Ivet.Model;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Ivet.Model.Library;

namespace Ivet.Services.Loaders
{
    public class LibrarySchemaLoaderService
    {
        private readonly ILogger _logger;

        public LibrarySchemaLoaderService(ILogger<LibrarySchemaLoaderService> logger)
        {
            _logger = logger;
        }

        public Schema Load(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Directory not found: {path}");

            var fullPath = Path.GetFullPath(path);
            var schema = new Schema();

            schema.Vertices.Add(typeof(Migration));

            var files = Directory.EnumerateFiles(fullPath, "*.dll").ToList();
            _logger.LogInformation("Loading {Count} DLL(s) from {Path}", files.Count, fullPath);

            files.ForEach(x =>
            {
                try
                {
                    _logger.LogDebug("  Loading: {FileName}", Path.GetFileName(x));
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
                    _logger.LogWarning("  Skipped: {FileName} (not introspectable)", Path.GetFileName(x));
                }
                catch (BadImageFormatException)
                {
                    _logger.LogWarning("  Skipped: {FileName} (not a valid .NET assembly)", Path.GetFileName(x));
                }
            });

            return schema;
        }
    }
}
