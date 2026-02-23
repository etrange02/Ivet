using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex]
    public class CompositeIndexVertex
    {
        [PropertyKey]
        [CompositeIndex("composite_idx", IsUnique = true)]
        public string IndexedProperty { get; set; } = string.Empty;
    }
}
