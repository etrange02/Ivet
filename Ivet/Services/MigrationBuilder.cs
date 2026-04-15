using Ivet.Extensions;
using Ivet.Model;
using Ivet.Model.Meta;

namespace Ivet.Services
{
    public class MigrationBuilder(MetaSchema metaSchema)
    {
        public const long DefaultIndexTimeoutMs = 300_000;

        public MetaSchema? MetaSchema { get; private set; } = metaSchema;
        public string? Description { get; set; }

        private readonly string skeleton = "graph.tx().rollback();" + Environment.NewLine +
            "mgmt = graph.openManagement();" + Environment.NewLine +
            "mgmt.getOpenInstances().forEach {" + Environment.NewLine +
            "if (it.reverse().take(1) != \")\") { mgmt.forceCloseInstance(it); } };" + Environment.NewLine +
            "mgmt.commit();" + Environment.NewLine +
            "int size = graph.getOpenTransactions().size();" + Environment.NewLine +
            "for (i = 0; i < size; i++) { graph.getOpenTransactions().getAt(0).rollback();};" + Environment.NewLine +
            "mgmt = graph.openManagement();" + Environment.NewLine +
            "%CONTENT%" + Environment.NewLine +
            "mgmt.commit();" + Environment.NewLine +
            "graph.tx().commit();";

        public List<(string Script, long? EvaluationTimeout)> Build()
        {
            var results = new List<(string, long?)>();
            results.Add((BuildMain(), null));
            results.AddRange(BuildCompositeIndices().Select(s => (s, (long?)DefaultIndexTimeoutMs)));
            results.AddRange(BuildMixedIndices().Select(s => (s, (long?)DefaultIndexTimeoutMs)));
            results.AddRange(BuildIndexBindings().Select(s => (s, (long?)DefaultIndexTimeoutMs)));

            return results;
        }

        private string BuildMain()
        {
            if (MetaSchema == null) return string.Empty;
            if (!MetaSchema.Vertices.Any() && !MetaSchema.Edges.Any() && !MetaSchema.Properties.Any() && !MetaSchema.VertexPropertyBindings.Any() && !MetaSchema.EdgePropertyBindings.Any() && !MetaSchema.Connections.Any()) return string.Empty;
            
            var content = string.Empty;

            content += $"// Vertices{Environment.NewLine}";
            content += string.Join(Environment.NewLine, MetaSchema.Vertices.ConvertAll(x => $"mgmt.makeVertexLabel('{x.Name}'){(x.Partitioned ? ".partition()" : "")}{(x.Static ? ".setStatic()" : "")}.make();"));
            content += $"{Environment.NewLine}";

            content += $"// Edges{Environment.NewLine}";
            content += string.Join(Environment.NewLine, MetaSchema.Edges.ConvertAll(x => $"mgmt.makeEdgeLabel('{x.Name}').multiplicity({x.Multiplicity.ToJavaString()}).make();"));
            content += $"{Environment.NewLine}";

            content += $"// Properties{Environment.NewLine}";
            content += string.Join(Environment.NewLine, MetaSchema.Properties.ConvertAll(x => $"mgmt.makePropertyKey('{x.Name}').dataType({x.DataType}).cardinality({x.Cardinality.ToJavaString()}).make();"));
            content += $"{Environment.NewLine}";

            content += $"// Vertex property bindings{Environment.NewLine}";
            content += string.Join(Environment.NewLine, MetaSchema.VertexPropertyBindings.ConvertAll(x => $"vertex = mgmt.getVertexLabel('{x.Entity}');prop = mgmt.getPropertyKey('{x.Name}');mgmt.addProperties(vertex, prop);"));
            content += $"{Environment.NewLine}";

            content += $"// Edge property bindings{Environment.NewLine}";
            content += string.Join(Environment.NewLine, MetaSchema.EdgePropertyBindings.ConvertAll(x => $"edge = mgmt.getEdgeLabel('{x.Entity}');prop = mgmt.getPropertyKey('{x.Name}');mgmt.addProperties(edge, prop);"));
            content += $"{Environment.NewLine}";

            content += $"// Connections{Environment.NewLine}";
            content += string.Join(Environment.NewLine, MetaSchema.Connections.ConvertAll(x => $"input = mgmt.getVertexLabel('{x.Ingoing}');output = mgmt.getVertexLabel('{x.Outgoing}');edge = mgmt.getEdgeLabel('{x.Edge}');mgmt.addConnection(edge, output, input);"));
            content += $"{Environment.NewLine}";

            return content;
        }

        private List<string> BuildCompositeIndices()
        {
            if (MetaSchema?.CompositeIndexes == null || !MetaSchema.CompositeIndexes.Any()) return new List<string>();

            var content = string.Empty;
            content += $"// Composite Indices (idempotent: skip creation if index already exists){Environment.NewLine}";
            foreach (var x in MetaSchema.CompositeIndexes)
            {
                content += $"if (mgmt.getGraphIndex('{x.Name}') == null) {{";
                content += BuildIndex(x, MetaSchema.IndexBindings.Where(p => p.IndexName == x.Name), ci => $"vertex = mgmt.getVertexLabel('{ci.IndexedElement}');index = mgmt.buildIndex('{ci.Name}', {ci.Kind}).indexOnly(vertex){(ci.IsUnique ? ".unique()" : "")}");
                content += $".buildCompositeIndex();";
                content += $"}}";
                content += $"{Environment.NewLine}";
            }
            content += BuildIndexActivation(MetaSchema.CompositeIndexes.Select(x => x.Name));

            return [content];
        }

        private List<string> BuildMixedIndices()
        {
            if (MetaSchema?.MixedIndexes == null || !MetaSchema.MixedIndexes.Any()) return new List<string>();

            var content = string.Empty;
            content += $"// Mixed Indices (idempotent: skip creation if index already exists){Environment.NewLine}";
            foreach (var x in MetaSchema.MixedIndexes)
            {
                content += $"if (mgmt.getGraphIndex('{x.Name}') == null) {{";
                if (!string.IsNullOrEmpty(x.IndexedElement))
                    content += BuildIndex(x, MetaSchema.IndexBindings.Where(p => p.IndexName == x.Name), mi => $"vertex = mgmt.getVertexLabel('{mi.IndexedElement}');index = mgmt.buildIndex('{mi.Name}', {mi.Kind}).indexOnly(vertex)");
                else
                    content += BuildIndex(x, MetaSchema.IndexBindings.Where(p => p.IndexName == x.Name), mi => $"index = mgmt.buildIndex('{mi.Name}', {mi.Kind})");
                content += $".buildMixedIndex('{x.BackendIndex}');";
                content += $"}}";
                content += $"{Environment.NewLine}";
            }
            content += BuildIndexActivation(MetaSchema.MixedIndexes.Select(x => x.Name));

            return [content];
        }

        private IEnumerable<string> BuildIndexBindings() => MetaSchema?.IndexBindings
                .Where(x => !MetaSchema.CompositeIndexes.Any(y => y.Name == x.IndexName) && !MetaSchema.MixedIndexes.Any(y => y.Name == x.IndexName))
                .GroupBy(x => x.IndexName)
                .Select(y =>
                {
                    var content = string.Empty;
                    // Extending an existing mixed/composite index: must go through
                    // mgmt.addIndexKey(idx, key, ...) — the JanusGraphIndex wrapper returned by
                    // mgmt.getGraphIndex(...) does not expose addKey() at runtime (it is only
                    // available on the builder produced by mgmt.buildIndex(...)). Each
                    // addIndexKey call is wrapped in an idempotency guard and followed by a
                    // commit + fresh openManagement so the new key is visible to the activation
                    // block.
                    content += string.Join(Environment.NewLine, y.Select(z => $"prop = mgmt.getPropertyKey('{z.PropertyName}');if (!mgmt.getGraphIndex('{z.IndexName}').getFieldKeys().toList().contains(prop)) {{ mgmt.addIndexKey(mgmt.getGraphIndex('{z.IndexName}'), prop{(z.Mapping != MappingType.NULL ? ", Mapping." + z.Mapping + ".asParameter()" : "")}); mgmt.commit(); mgmt = graph.openManagement(); }}"));
                    content += BuildIndexActivation([y.Key]);
                    return content;
                }) ?? new List<string>();

        /// <summary>
        /// Generates idempotent Groovy for index activation (runs as its own script).
        /// Uses the mgmt already opened by the skeleton. Applies to both composite and
        /// mixed indexes (creation and extension paths).
        /// State machine per index:
        ///   ENABLED  → skip (already done)
        ///   INSTALLED → REGISTER_INDEX + commit + await REGISTERED + REINDEX + commit
        ///   REGISTERED → REINDEX + commit
        /// The <c>awaitGraphIndexStatus(REGISTERED)</c> between REGISTER and REINDEX is
        /// essential: REGISTER_INDEX only schedules the transition, so the subsequent
        /// REINDEX must not run against a still-INSTALLED key (it silently no-ops).
        /// Without the await, mixed-index extensions (addIndexKey on an existing ENABLED
        /// mixed index) leave the new key stuck in INSTALLED with no visible error.
        /// On a timeout (stuck open instance, cluster not converging) the script fails
        /// with a clear diagnostic; the operator can restart JanusGraph and let Ivet retry.
        /// Not awaiting ENABLED at the end: REINDEX on large composite indexes can exceed
        /// the 1m default, and downstream scripts don't depend on reindex completion — the
        /// next Ivet pass picks up from REGISTERED → REINDEX cleanly.
        /// </summary>
        private static string BuildIndexActivation(IEnumerable<string> indexNames)
        {
            var content = string.Empty;

            foreach (var name in indexNames)
            {
                content += $"// Index activation: {name} (idempotent){Environment.NewLine}";
                content += $"idx = mgmt.getGraphIndex('{name}');";
                content += $"allEnabled = idx.getFieldKeys().every {{ pk -> idx.getIndexStatus(pk) == SchemaStatus.ENABLED }};";
                content += $"{Environment.NewLine}";
                content += $"if (!allEnabled) {{";
                content += $"{Environment.NewLine}";
                // INSTALLED → REGISTER_INDEX to start transition, then await REGISTERED.
                content += $"hasInstalled = idx.getFieldKeys().any {{ pk -> idx.getIndexStatus(pk) == SchemaStatus.INSTALLED }};";
                content += $"{Environment.NewLine}";
                content += $"if (hasInstalled) {{ mgmt.updateIndex(idx, SchemaAction.REGISTER_INDEX).get(); mgmt.commit(); org.janusgraph.graphdb.database.management.ManagementSystem.awaitGraphIndexStatus(graph, '{name}').status(SchemaStatus.REGISTERED).call(); mgmt = graph.openManagement(); }}";
                content += $"{Environment.NewLine}";
                // REGISTERED → REINDEX. No await ENABLED afterwards: for large composite indexes
                // the default 1m timeout would fail unnecessarily; Ivet's retry resumes cleanly.
                content += $"mgmt.updateIndex(mgmt.getGraphIndex('{name}'), SchemaAction.REINDEX).get();";
                content += $"{Environment.NewLine}";
                content += $"mgmt.commit();";
                content += $"{Environment.NewLine}";
                content += $"mgmt = graph.openManagement();";
                content += $"{Environment.NewLine}";
                content += $"}}";
                content += $"{Environment.NewLine}";
            }

            return content;
        }

        private static string BuildIndex<T>(T graphIndex, IEnumerable<MetaIndexBinding> properties, Func<T, string> convert)
        {
            var result = string.Empty;

            foreach (var (p, index) in properties.Select((v, i) => (v, i)))
            {
                result += $"prop_{index} = mgmt.getPropertyKey('{p.PropertyName}');";
            }
            result += string.Join(string.Empty, properties.Select(p => $""));

            result += convert(graphIndex);
            result += string.Join(string.Empty, properties.Select(p => $""));

            foreach (var (p, index) in properties.Select((v, i) => (v, i)))
            {
                result += $".addKey(prop_{index}{(p.Mapping != MappingType.NULL ? ", Mapping." + p.Mapping + ".asParameter()" : "")})";
            }
            return result;
        }

        public MigrationFile BuildFileContent()
        {
            return new MigrationFile
            {
                Description = Description ?? string.Empty,
                Scripts = Build().Where(x => !string.IsNullOrEmpty(x.Script)).Select(x =>
                {
                    return new MigrationScript
                    {
                        Script = skeleton.Replace("%CONTENT%", x.Script).Replace($"{Environment.NewLine}", $"{Environment.NewLine}   "),
                        EvaluationTimeout = x.EvaluationTimeout
                    };
                }).ToList()
            };
        }

        public List<MigrationFile> BuildFileContents()
        {
            return Build().Where(x => !string.IsNullOrEmpty(x.Script)).Select(x =>
            {
                return new MigrationFile
                {
                    Description = Description ?? string.Empty,
                    Content = skeleton.Replace("%CONTENT%", x.Script).Replace($"{Environment.NewLine}", $"{Environment.NewLine}   "),
                    EvaluationTimeout = x.EvaluationTimeout
                };
        }).ToList();
        }
    }
}
