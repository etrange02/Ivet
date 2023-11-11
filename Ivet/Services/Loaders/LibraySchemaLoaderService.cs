using Ivet.Model;
using System.Reflection;
using Ivet.Model.Library;

namespace Ivet.Services.Loaders
{
    public class LibraySchemaLoaderService
    {
        public Schema Load(string path)
        {
            var schema = new Schema();

            var files = Directory.EnumerateFiles(path, "*.dll").ToList();
            files.ForEach(x =>
            {
                try
                {
                    var assembly = Assembly.LoadFrom(x);
                    var graphClasses = assembly.GetTypes().Where(t => t.GetCustomAttributes<AbstractGraphItemAttribute>().Any()).ToList();
                    if (graphClasses.Any())
                    {
                        schema.Vertices.AddRange(graphClasses.Where(t => t.GetCustomAttributes<VertexAttribute>().Any()));
                        schema.Edges.AddRange(graphClasses.Where(t => t.GetCustomAttributes<EdgeAttribute>().Any()));
                    }
                }
                catch { }
            });

            return schema;
        }
    }
}
