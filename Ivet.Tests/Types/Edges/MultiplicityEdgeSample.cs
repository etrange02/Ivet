using Ivet.Model;
using Ivet.Tests.Types.Vertices;

namespace Ivet.Tests.Types.Edges
{
    [Edge(typeof(VertexSample), typeof(NamedVertexSample), Multiplicity = Multiplicity.ONE2MANY)]
    public class MultiplicityEdgeSample
    {
    }
}
