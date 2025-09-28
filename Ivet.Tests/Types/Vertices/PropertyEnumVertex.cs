using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex]
    public class PropertyEnumVertex
    {
        [PropertyKey]
        public EnumProperty Property { get; set; }
    }
}
