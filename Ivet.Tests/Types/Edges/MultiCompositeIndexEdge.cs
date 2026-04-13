using Ivet.Model;
using Ivet.Tests.Types.Vertices;

namespace Ivet.Tests.Types.Edges
{
    [Edge(typeof(VertexSample), typeof(VertexSample))]
    public class MultiCompositeIndexEdge
    {
        [PropertyKey]
        [CompositeIndex("edge_first")]
        [CompositeIndex("edge_second", IsUnique = true)]
        public string IndexedProperty { get; set; } = string.Empty;
    }
}
