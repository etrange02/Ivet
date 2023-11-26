using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex()]
    public class PropertyVertex
    {
        [PropertyKey()]
        public string MyProperty { get; set; }
    }
}
