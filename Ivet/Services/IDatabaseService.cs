using ExRam.Gremlinq.Core;

namespace Ivet.Services
{
    public interface IDatabaseService
    {
        IGremlinQuerySource GremlinqClient { get; }

        string GetConnectionSchema();
        string GetEdgeSchema();
        string GetEdgesPropertyBindingsSchema();
        string GetIndexBindingSchema();
        string GetIndexSchema();
        string GetIndexStatusSchema();
        string GetPropertyKeysSchema();
        string GetVertexPropertyBindingsSchema();
        string GetVertexSchema();
    }
}