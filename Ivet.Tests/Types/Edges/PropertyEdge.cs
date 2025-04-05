using Ivet.Model;
using Ivet.Tests.Types.Vertices;

namespace Ivet.Tests.Types.Edges
{
    [Edge(typeof(VertexSample), typeof(NamedVertexSample))]
    public class PropertyEdge
    {
        [PropertyKey]
        public string? MyProperty { get; set; }
        [PropertyKey]
        public IEnumerable<char> MyCharProperty { get; set; }
    }
}
