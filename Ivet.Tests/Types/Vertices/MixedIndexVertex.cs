using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex]
    public class MixedIndexVertex
    {
        [PropertyKey]
        [MixedIndex("mixed_idx", Backend = "search", Mapping = MappingType.TEXT)]
        public string SearchProperty { get; set; } = string.Empty;
    }
}
