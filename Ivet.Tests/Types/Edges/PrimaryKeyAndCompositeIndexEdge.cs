using Ivet.Model;
using Ivet.Tests.Types.Vertices;

namespace Ivet.Tests.Types.Edges
{
    [Edge(typeof(VertexSample), typeof(VertexSample))]
    public class PrimaryKeyAndCompositeIndexEdge
    {
        [PropertyKey]
        [PrimaryKey]
        [CompositeIndex("redundant_edge_idx")]
        public string Id { get; set; } = string.Empty;
    }
}
