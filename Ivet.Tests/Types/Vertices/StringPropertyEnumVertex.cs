using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex]
    public class StringPropertyEnumVertex
    {
        [PropertyKey(EnumAsString = true)]
        public EnumProperty Property { get; set; }
    }
}
