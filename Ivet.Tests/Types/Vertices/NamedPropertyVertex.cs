using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex()]
    public class NamedPropertyVertex
    {
        [PropertyKey(Name = "My property")]
        public string MyProperty { get; set; } = string.Empty;
    }
}
