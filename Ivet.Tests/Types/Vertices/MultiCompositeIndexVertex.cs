using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex]
    public class MultiCompositeIndexVertex
    {
        [PropertyKey]
        [CompositeIndex("first_idx")]
        [CompositeIndex("second_idx", IsUnique = true)]
        public string IndexedProperty { get; set; } = string.Empty;
    }
}
