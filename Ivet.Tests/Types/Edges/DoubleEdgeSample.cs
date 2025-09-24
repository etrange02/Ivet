using Ivet.Model;
using Ivet.Tests.Types.Vertices;

namespace Ivet.Tests.Types.Edges
{
    [Edge(typeof(VertexSample), typeof(StaticVertexSample))]
    [Edge(typeof(VertexSample), typeof(NamedVertexSample))]
    public class DoubleEdgeSample
    {
    }
}
