using Ivet.Model;
using Ivet.Tests.Types.Vertices;

namespace Ivet.Tests.Types.Edges
{
    [Edge(typeof(VertexSample), typeof(VertexSample))]
    public class PrimaryKeyEdge
    {
        [PropertyKey]
        [PrimaryKey]
        public string Id { get; set; } = string.Empty;
    }
}
