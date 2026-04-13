using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex]
    public class PrimaryKeyAndDistinctCompositeIndexVertex
    {
        [PropertyKey]
        [PrimaryKey]
        public string Id { get; set; } = string.Empty;

        [PropertyKey]
        [CompositeIndex("other_idx")]
        public string OtherProperty { get; set; } = string.Empty;
    }
}
