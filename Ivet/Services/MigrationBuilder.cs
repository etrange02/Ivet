using Ivet.Extensions;
using Ivet.Model;
using Ivet.Model.Meta;

namespace Ivet.Services
{
    public class MigrationBuilder
    {
        public MetaSchema MetaSchema { get; set; }

        private readonly string skeleton = "graph.tx().rollback();" + Environment.NewLine +
            "mgmt = graph.openManagement();" + Environment.NewLine +
            "mgmt.getOpenInstances().forEach {" + Environment.NewLine +
            "if (it.reverse().take(1) != \")\") { mgmt.forceCloseInstance(it); } };" + Environment.NewLine +
            "int size = graph.getOpenTransactions().size();" + Environment.NewLine +
            "for (i = 0; i < size; i++) { graph.getOpenTransactions().getAt(0).rollback();};" + Environment.NewLine +
            "mgmt = graph.openManagement();" + Environment.NewLine +
            "%CONTENT%" + Environment.NewLine +
            "mgmt.commit();" + Environment.NewLine +
            "graph.tx().commit();";

        public List<string> Build()
        {
            var results = new List<string>();
            results.Add(BuildMain());
            results.AddRange(BuildCompositeIndeces());
            results.AddRange(BuildMixedIndeces());
            results.AddRange(BuildIndexBindings());

            return results;
        }

        private string BuildMain()
        {
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

        private List<string> BuildCompositeIndeces()
        {
            return MetaSchema.CompositeIndexes.ConvertAll(x =>
            {
                var content = string.Empty;

                content += $"// Composite Indices{Environment.NewLine}";
                content += BuildIndex(x, MetaSchema.IndexBindings.Where(p => p.IndexName == x.Name), x => $"vertex = mgmt.getVertexLabel('{x.IndexedElement}');index = mgmt.buildIndex('{x.Name}', {x.Kind}).indexOnly(vertex){(x.IsUnique ? ".unique()" : "")}");
                content += $".buildCompositeIndex();";

                content += $"{Environment.NewLine}";
                content += "mgmt.commit();";
                content += $"{Environment.NewLine}";
                content += $"{Environment.NewLine}";

                content += $"// Index: Waiting for registered status{Environment.NewLine}";
                content += $"ManagementSystem.awaitGraphIndexStatus(graph, '{x.Name}').call();";
                content += $"{Environment.NewLine}";
                content += $"{Environment.NewLine}";

                content += $"// Index: Reindexing{Environment.NewLine}";
                content += $"mgmt = graph.openManagement();";
                content += $"{Environment.NewLine}";
                content += $"mgmt.updateIndex(mgmt.getGraphIndex('{x.Name}'), SchemaAction.REINDEX).get();";

                return content;
            });
        }

        private List<string> BuildMixedIndeces()
        {
            return MetaSchema.MixedIndexes.ConvertAll(x =>
            {
                var content = string.Empty;
                content += $"// Mixed Indices{Environment.NewLine}";
                content += BuildIndex(x, MetaSchema.IndexBindings.Where(p => p.IndexName == x.Name), x => $"vertex = mgmt.getVertexLabel('{x.IndexedElement}');index = mgmt.buildIndex('{x.Name}', {x.Kind}).indexOnly(vertex)");
                content += $".buildMixedIndex('{x.BackendIndex}');";

                content += $"{Environment.NewLine}";
                content += "mgmt.commit();";
                content += $"{Environment.NewLine}";
                content += $"{Environment.NewLine}";

                content += $"// Index: Waiting for registered status{Environment.NewLine}";
                content += string.Join(Environment.NewLine, MetaSchema.MixedIndexes.Select(x => $"ManagementSystem.awaitGraphIndexStatus(graph, '{x.Name}').call();"));
                content += $"{Environment.NewLine}";
                content += $"{Environment.NewLine}";

                content += $"// Index: Reindexing{Environment.NewLine}";
                content += $"mgmt = graph.openManagement();";
                content += $"{Environment.NewLine}";
                content += string.Join(Environment.NewLine, MetaSchema.MixedIndexes.Select(x => $"mgmt.updateIndex(mgmt.getGraphIndex('{x.Name}'), SchemaAction.REINDEX).get();"));

                return content;
            });
        }

        private IEnumerable<string> BuildIndexBindings()
        {
            return MetaSchema.IndexBindings
                .Where(x => !MetaSchema.CompositeIndexes.Any(y => y.Name == x.IndexName) && !MetaSchema.MixedIndexes.Any(y => y.Name == x.IndexName))
                .GroupBy(x => x.IndexName)
                .Select(y => {
                    var content = string.Empty;

                    content += string.Join(Environment.NewLine, y.Select(z => $"prop = mgmt.getPropertyKey('{z.PropertyName}');index = mgmt.getGraphIndex('{z.IndexName}').addKey(prop{(z.Mapping != MappingType.NULL ? ", Mapping." + z.Mapping + ".asParameter()" : "")});"));

                    content += $"{Environment.NewLine}";
                    content += "mgmt.commit();";
                    content += $"{Environment.NewLine}";
                    content += $"{Environment.NewLine}";

                    content += $"// Index: Waiting for registered status{Environment.NewLine}";
                    content += $"ManagementSystem.awaitGraphIndexStatus(graph, '{y.Key}').call();";
                    content += $"{Environment.NewLine}";
                    content += $"{Environment.NewLine}";

                    content += $"// Index: Reindexing{Environment.NewLine}";
                    content += $"mgmt = graph.openManagement();";
                    content += $"{Environment.NewLine}";
                    content += $"mgmt.updateIndex(mgmt.getGraphIndex('{y.Key}'), SchemaAction.REINDEX).get();";

                    return content;
                });
        }

        private string BuildIndex<T>(T graphIndex, IEnumerable<MetaIndexBinding> properties, Func<T, string> convert)
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

        public List<MigrationFile> BuildFileContent()
        {
            return Build().Where(x => !string.IsNullOrEmpty(x)).Select(x =>
            {
                return new MigrationFile
                {
                    Content = skeleton.Replace("%CONTENT%", x).Replace($"{Environment.NewLine}", $"{Environment.NewLine}   ")
                };
            }).ToList();
        }
    }
}
