using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    public abstract class SharedMixedIndexBase
    {
        [PropertyKey]
        [MixedIndex("shared_idx", Backend = "search", Mapping = MappingType.TEXT)]
        public string SharedName { get; set; } = string.Empty;

        [PropertyKey]
        [MixedIndex("shared_idx", Backend = "search", Mapping = MappingType.STRING)]
        public string SharedCode { get; set; } = string.Empty;
    }

    [Vertex]
    public class SharedMixedIndexVertexA : SharedMixedIndexBase
    {
    }

    [Vertex]
    public class SharedMixedIndexVertexB : SharedMixedIndexBase
    {
    }
}
