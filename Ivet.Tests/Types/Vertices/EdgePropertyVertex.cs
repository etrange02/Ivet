using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex]
    public class EdgePropertyVertex
    {
        [EdgeProperty]
        public List<VertexSample> Related { get; set; } = new();
    }
}
