using Ivet.Model.Database;

namespace Ivet.Services.Loaders
{
    public class DatabaseSchemaLoaderService
    {
        private readonly IDatabaseService _databaseService;

        public DatabaseSchemaLoaderService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public Schema Load()
        {
            var vertexSchema = _databaseService.GetVertexSchema();
            var edgeSchema = _databaseService.GetEdgeSchema();
            var propertyKeySchema = _databaseService.GetPropertyKeysSchema();
            var connectionSchema = _databaseService.GetConnectionSchema();
            var vertexPropertyBindingSchema = _databaseService.GetVertexPropertyBindingsSchema();
            var edgePropertyBindingSchema = _databaseService.GetEdgesPropertyBindingsSchema();
            var indices = _databaseService.GetIndexSchema();
            var indexBindings = _databaseService.GetIndexBindingSchema();

            var parser = new Parser();

            var schema = new Schema();
            schema.Vertices.AddRange(parser.GetVertices(vertexSchema));
            schema.Edges.AddRange(parser.GetEdges(edgeSchema));
            schema.PropertyKeys.AddRange(parser.GetPropertyKeys(propertyKeySchema));
            schema.Connections.AddRange(parser.GetConnections(connectionSchema));
            schema.VertexPropertyBindings.AddRange(parser.GetPropertyBindings(vertexPropertyBindingSchema));
            schema.EdgesPropertyBindings.AddRange(parser.GetPropertyBindings(edgePropertyBindingSchema));
            schema.Indices.AddRange(parser.GetIndices(indices));
            schema.IndexBindings.AddRange(parser.GetIndexBindings(indexBindings));

            return schema;
        }
    }
}
