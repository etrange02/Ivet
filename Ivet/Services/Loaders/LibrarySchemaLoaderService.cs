using Ivet.Model;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Runtime.Loader;
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

            // Isolated load context so the consumer's runtime dependencies (Gremlin.Net, ExRam, …)
            // never clash with the ones Ivet itself already loaded. Two versions of the same assembly
            // coexist across contexts, so a version mismatch that aborted Assembly.LoadFrom (manifest
            // mismatch, FileLoadException) simply loads side by side here. The ONE contract we must
            // share with the host is Ivet.Model: the converter matches [Vertex]/[PropertyKey]/… by
            // runtime type identity, so the model's attributes have to be the tool's own attribute
            // types, not a copy loaded from the directory. InspectionLoadContext.Load returns null for
            // Ivet.Model (falls back to the default context = the tool's) and the Ivet.Model.dll in the
            // directory is skipped below so no second copy is ever loaded. Ivet.Model is not strong-named,
            // so this stays agnostic to the version the consumer compiled against.
            var context = new InspectionLoadContext(fullPath);

            files.ForEach(x =>
            {
                // Never load the directory's own Ivet.Model: it must come from the host (see above).
                if (string.Equals(Path.GetFileNameWithoutExtension(x), "Ivet.Model", StringComparison.OrdinalIgnoreCase))
                    return;

                try
                {
                    _logger.LogDebug("  Loading: {FileName}", Path.GetFileName(x));
                    var assembly = context.LoadFromAssemblyPath(x);
                    // GetTypes() is all-or-nothing: one unloadable type (e.g. a service referencing a
                    // dependency absent from --dir, or at a diverging version) throws and would drop the
                    // whole assembly. The [Vertex]/[Edge] model types never reference those dependencies,
                    // so keep the types that did load and ignore the rest — ivet stays agnostic to the
                    // consumer's runtime dependencies.
                    Type[] loadedTypes;
                    try { loadedTypes = assembly.GetTypes(); }
                    catch (ReflectionTypeLoadException ex) { loadedTypes = ex.Types.Where(t => t is not null).ToArray()!; }
                    var graphClasses = loadedTypes.Where(t => t.GetCustomAttributes<AbstractGraphItemAttribute>().Any()).ToList();
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
                catch (FileLoadException)
                {
                    // A dependency the assembly binds to could not be resolved/loaded at a compatible
                    // version. The model types don't need it; keep going with the assemblies that load.
                    _logger.LogWarning("  Skipped: {FileName} (dependency load conflict)", Path.GetFileName(x));
                }
            });

            return schema;
        }

        /// <summary>
        /// Loads the inspected assemblies in isolation from the tool's own dependencies, sharing only the
        /// Ivet.Model contract with the host so attribute type identity is preserved for the converter.
        /// </summary>
        private sealed class InspectionLoadContext : AssemblyLoadContext
        {
            private readonly string _directory;

            public InspectionLoadContext(string directory) : base(name: "IvetInspection", isCollectible: false)
            {
                _directory = directory;
            }

            protected override Assembly? Load(AssemblyName assemblyName)
            {
                // Share the Ivet.Model contract (and any framework/host assembly) with the default
                // context: returning null lets the runtime resolve it from the host, so [Vertex] &co.
                // keep the tool's own attribute identity.
                if (string.Equals(assemblyName.Name, "Ivet.Model", StringComparison.OrdinalIgnoreCase))
                    return null;

                // Prefer the consumer's own copy of a dependency from the directory (isolated), so its
                // version can diverge from the tool's without conflict.
                if (assemblyName.Name is { } name)
                {
                    var candidate = Path.Combine(_directory, name + ".dll");
                    if (File.Exists(candidate))
                        return LoadFromAssemblyPath(candidate);
                }

                // Framework / unresolved deps fall back to the default context.
                return null;
            }
        }
    }
}
