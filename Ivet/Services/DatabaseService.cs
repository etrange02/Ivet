using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.JanusGraph;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using Gremlin.Net.Driver;
using Ivet.Model;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace Ivet.Services
{
    public class DatabaseService : IDisposable, IDatabaseService
    {
        private GremlinClient _client;
        private bool disposedValue;

        public IGremlinQuerySource GremlinqClient { get; private set; }

        public DatabaseService(string ipAddress, int port)
        {
            _client = new GremlinClient(new GremlinServer(ipAddress, port));

            var uri = new Uri($"ws://{ipAddress}:{port}");

            GremlinqClient = g.UseJanusGraph<AbstractVertex, AbstractEdge>(configurator => configurator
                                .At(uri)
                                .UseNewtonsoftJson());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés)
                }

                // TODO: libérer les ressources non managées (objets non managés) et substituer le finaliseur
                // TODO: affecter aux grands champs une valeur null
                _client?.Dispose();
                _client = null;
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

        public string Execute(string request)
        {
            return _client.SubmitAsync<string>(request).Result.Single();
        }
    }
}
