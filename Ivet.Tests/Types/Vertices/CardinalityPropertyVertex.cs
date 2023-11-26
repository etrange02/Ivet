using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex()]
    public class CardinalityPropertyVertex
    {
        [PropertyKey(Cardinality = Cardinality.LIST)]
        public string MyProperty { get; set; }
    }
}
