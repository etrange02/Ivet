using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Ivet.Model;

namespace Ivet.Services
{
    public class DatabaseService : IDisposable, IDatabaseService
    {
        private GremlinClient? _client;
        private bool disposedValue;

        public DatabaseService(string ipAddress, int port, bool useSsl = false)
        {
            _client = new GremlinClient(new GremlinServer(ipAddress, port, enableSsl: useSsl));
        }

        // Migration tracking (read/write the `Migration` vertex) in raw Gremlin — ivet carries no
        // ExRam.Gremlinq, so it stays agnostic to whatever ExRam version the consumer's model DLLs use.
        // Raw-script labels/keys are bound via nameof so a model rename breaks the build, not the query.
        public IReadOnlyList<AppliedMigration> GetAppliedMigrations(IEnumerable<string> names)
        {
            const int chunkSize = 200;
            var distinct = names.Distinct().ToList();
            var applied = new List<AppliedMigration>();
            if (distinct.Count == 0) return applied;

            // hasLabel('Migration') lets JanusGraph use the Migration primary-key composite index
            // (indexOnly(Migration)) instead of a full vertex scan; within() filters server-side.
            // Chunked at 200 to stay under the Gremlin parameter / WebSocket frame limits. elementMap
            // returns only the present keys, so a vertex without MigrationDate still parses.
            const string script = "g.V().hasLabel('" + nameof(Migration) + "')"
                + ".has('" + nameof(Migration.MigrationName) + "', within(mNames))"
                + ".elementMap('" + nameof(Migration.MigrationName) + "','" + nameof(Migration.MigrationDate) + "')";

            foreach (var chunk in distinct.Chunk(chunkSize))
            {
                var bindings = new Dictionary<string, object> { ["mNames"] = chunk.ToList() };
                var rows = _client!.SubmitAsync<Dictionary<object, object>>(script, bindings).Result;
                foreach (var row in rows)
                {
                    var name = GetValue(row, nameof(Migration.MigrationName))?.ToString();
                    if (string.IsNullOrEmpty(name)) continue;
                    applied.Add(new AppliedMigration(name, ParseDate(GetValue(row, nameof(Migration.MigrationDate)))));
                }
            }
            return applied;
        }

        public void AddAppliedMigration(string name, DateTime date)
        {
            const string script = "g.addV('" + nameof(Migration) + "')"
                + ".property('" + nameof(Migration.MigrationName) + "', mName)"
                + ".property('" + nameof(Migration.MigrationDate) + "', mDate)";
            var bindings = new Dictionary<string, object> { ["mName"] = name, ["mDate"] = date };
            _client!.SubmitAsync(script, bindings).Wait();
        }

        private static object? GetValue(Dictionary<object, object> row, string key)
            => row.FirstOrDefault(kv => kv.Key?.ToString() == key).Value;

        private static DateTime? ParseDate(object? value) => value switch
        {
            null => null,
            DateTime dt => dt,
            DateTimeOffset dto => dto.LocalDateTime,
            long epochMs => DateTimeOffset.FromUnixTimeMilliseconds(epochMs).LocalDateTime,
            _ => DateTime.TryParse(value.ToString(), out var parsed) ? parsed : null,
        };

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client?.Dispose();
                    _client = null;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void GenerateData()
        {
            _client.SubmitAsync("mgmt = graph.openManagement(); v1 = mgmt.makeVertexLabel('vertex_1').make();mgmt.commit();").Wait();
            _client.SubmitAsync("mgmt = graph.openManagement(); v2 = mgmt.makeVertexLabel('vertex_2').make();mgmt.commit();").Wait();
            _client.SubmitAsync("mgmt = graph.openManagement(); e_1_2 = mgmt.makeEdgeLabel('edge_v1_v2').make();mgmt.commit();").Wait();
            _client.SubmitAsync("mgmt = graph.openManagement(); mgmt.makePropertyKey('my_prop').dataType(String.class).cardinality(Cardinality.SINGLE).make();mgmt.commit();").Wait();
            _client.SubmitAsync("mgmt = graph.openManagement(); mgmt.makePropertyKey('my_prop2').dataType(String.class).cardinality(Cardinality.SINGLE).make();mgmt.commit();").Wait();
            _client.SubmitAsync("mgmt = graph.openManagement(); v1 = mgmt.getVertexLabel('vertex_1'); my_prop = mgmt.getPropertyKey('my_prop'); mgmt.addProperties(v1, my_prop); mgmt.commit();").Wait();
            _client.SubmitAsync("mgmt = graph.openManagement(); v2 = mgmt.getVertexLabel('vertex_2'); my_prop = mgmt.getPropertyKey('my_prop2'); mgmt.addProperties(v2, my_prop); mgmt.commit();").Wait();
            _client.SubmitAsync("mgmt = graph.openManagement(); v1 = mgmt.getVertexLabel('vertex_1'); v2 = mgmt.getVertexLabel('vertex_2'); e_1_2 = mgmt.getEdgeLabel('edge_v1_v2'); mgmt.addConnection(e_1_2, v1, v2); mgmt.commit();").Wait();
            _client.SubmitAsync("mgmt = graph.openManagement(); v1 = mgmt.getVertexLabel('vertex_1'); my_prop = mgmt.getPropertyKey('my_prop'); mgmt.buildIndex('byNameUnique', Vertex.class).addKey(my_prop).indexOnly(v1).unique().buildCompositeIndex(); mgmt.commit(); ManagementSystem.awaitGraphIndexStatus(graph, 'byNameUnique').call(); mgmt = graph.openManagement(); mgmt.updateIndex(mgmt.getGraphIndex('byNameUnique'), SchemaAction.REINDEX).get(); mgmt.commit()").Wait();
            _client.SubmitAsync("mgmt = graph.openManagement(); v2 = mgmt.getVertexLabel('vertex_2'); my_prop = mgmt.getPropertyKey('my_prop2'); mgmt.buildIndex('byName_mixed', Vertex.class).addKey(my_prop, Mapping.TEXT.asParameter()).indexOnly(v2).buildMixedIndex('search'); mgmt.commit(); ManagementSystem.awaitGraphIndexStatus(graph, 'byName_mixed').call(); mgmt = graph.openManagement(); mgmt.updateIndex(mgmt.getGraphIndex('byName_mixed'), SchemaAction.REINDEX).get(); mgmt.commit()").Wait();
        }

        public string GetVertexSchema()
        {
            return _client.SubmitAsync<string>("mgmt = graph.openManagement(); " +
                "mgmt.printVertexLabels()").Result.Single();
        }

        public string GetEdgeSchema()
        {
            return _client.SubmitAsync<string>("mgmt = graph.openManagement(); " +
                "mgmt.printEdgeLabels()").Result.Single();
        }

        public string GetPropertyKeysSchema()
        {
            return _client.SubmitAsync<string>("mgmt = graph.openManagement(); " +
                "mgmt.printPropertyKeys()").Result.Single();
        }

        public string GetConnectionSchema()
        {
            var res = _client.SubmitAsync<string>(
               "mgmt = graph.openManagement(); " +
               $"result = \"|Edge|Ingoing|Outgoing|\\n\" ;" +
                "edges = mgmt.getRelationTypes(EdgeLabel.class);" +
                "for (edgeLabel in edges) {" +
                    " edgeLabel.mappedConnections().each() { connection -> " +
                        $" result += '|' << connection.getEdgeLabel() << '|' << connection.getIncomingVertexLabel().name() << '|' << connection.getOutgoingVertexLabel().name() << '|\\n';" +
                    "};" +
                "};" +
                "return result;"
                ).Result.Single();
            return res;
        }

        public string GetVertexPropertyBindingsSchema()
        {
            var res = _client.SubmitAsync<string>(
               "mgmt = graph.openManagement(); " +
               $"result = \"|Name|Entity|\\n\" ;" +
                "vertices = mgmt.getVertexLabels();" +
                "for (vertexLabel in vertices) {" +
                    " vertexLabel.mappedProperties().each() { property -> " +
                        $" result += '|' << property.name() << '|' << vertexLabel.name() << '|\\n';" +
                    "};" +
                "};" +
                "return result;"
                ).Result.Single();
            return res;
        }

        public string GetEdgesPropertyBindingsSchema()
        {
            var res = _client.SubmitAsync<string>(
               "mgmt = graph.openManagement(); " +
               $"result = \"|Name|Entity|\\n\" ;" +
                "edges = mgmt.getRelationTypes(EdgeLabel.class);" +
                "for (edgeLabel in edges) {" +
                    " edgeLabel.mappedProperties().each() { property -> " +
                        $" result += '|' << property.name() << '|' << edgeLabel.name() << '|\\n';" +
                    "};" +
                "};" +
                "return result;"
                ).Result.Single();
            return res;
        }

        public string GetIndexSchema()
        {
            var res = _client.SubmitAsync<string>(
               "mgmt = graph.openManagement(); " +
               $"result = \"|Name|IsUnique|IsMixedIndex|IsCompositeIndex|BackendIndex|IndexedElement|\\n\" ;" +
                "indexex = mgmt.getGraphIndexes(Vertex.class);" +
                "for (index in indexex) {" +
                    $" result += '|' << index.name() << '|' << index.isUnique() << '|' << index.isMixedIndex() << '|' << index.isCompositeIndex() << '|' << index.getBackingIndex() << '|' << index.getIndexedElement() << '|\\n';" +
                "};" +
                "indexex = mgmt.getGraphIndexes(Edge.class);" +
                "for (index in indexex) {" +
                    $" result += '|' << index.name() << '|' << index.isUnique() << '|' << index.isMixedIndex() << '|' << index.isCompositeIndex() << '|' << index.getBackingIndex() << '|' << index.getIndexedElement() << '|\\n';" +
                "};" +
                "return result;"
                ).Result.Single();
            return res;
        }

        public string GetIndexStatusSchema()
        {
            var res = _client.SubmitAsync<string>(
               "mgmt = graph.openManagement(); " +
               $"result = \"|IndexName|IndexType|IsUnique|PropertyName|DataType|Cardinality|Status|\\n\" ;" +
                "for (cls in [Vertex.class, Edge.class]) {" +
                    "for (idx in mgmt.getGraphIndexes(cls)) {" +
                        "indexType = idx.isMixedIndex() ? 'mixed' : 'composite';" +
                        "for (pk in idx.getFieldKeys()) {" +
                            $" result += '|' << idx.name() << '|' << indexType << '|' << idx.isUnique() << '|' << pk.name() << '|' << pk.dataType().getSimpleName() << '|' << pk.cardinality() << '|' << idx.getIndexStatus(pk) << '|\\n';" +
                        "};" +
                    "};" +
                "};" +
                "return result;"
                ).Result.Single();
            return res;
        }

        public string GetIndexBindingSchema()
        {
            var res = _client.SubmitAsync<string>(
               "mgmt = graph.openManagement(); " +
               $"result = \"|IndexName|PropertyName|Parameter|\\n\" ;" +
                "indexes = mgmt.getGraphIndexes(Vertex.class);" +
                "for (index in indexes) {" +
                    "for (property in index.getFieldKeys()) {" +
                        "parameters = index.getParametersFor(property);" +
                        "if (parameters.size() == 0) {" +
                            $" result += '|' << index.name() << '|' << property.name() << '|' << '|\\n';" +
                        "} else {" +
                            "for (parameter in parameters) {" +
                                $" result += '|' << index.name() << '|' << property.name() << '|' << parameter.value() << '|\\n';" +
                            "};" +
                        "};" +
                    "};" +
                "};" +
                "return result;"
                ).Result.Single();
            return res;
        }

        public string Execute(string request, long? evaluationTimeout = null)
        {
            if (evaluationTimeout.HasValue)
            {
                var msg = RequestMessage.Build(Tokens.OpsEval)
                    .AddArgument(Tokens.ArgsGremlin, request)
                    .AddArgument(Tokens.ArgsEvalTimeout, evaluationTimeout.Value)
                    .Create();
                return _client.SubmitAsync<string>(msg).Result.Single();
            }
            return _client.SubmitAsync<string>(request).Result.Single();
        }
    }
}
