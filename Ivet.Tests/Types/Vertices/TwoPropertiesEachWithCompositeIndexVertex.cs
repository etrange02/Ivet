using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex]
    public class TwoPropertiesEachWithCompositeIndexVertex
    {
        [PropertyKey]
        [CompositeIndex("idx_a")]
        public string PropA { get; set; } = string.Empty;

        [PropertyKey]
        [CompositeIndex("idx_b", IsUnique = true)]
        public string PropB { get; set; } = string.Empty;
    }
}
