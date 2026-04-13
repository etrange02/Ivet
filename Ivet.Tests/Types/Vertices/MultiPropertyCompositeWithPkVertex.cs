using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex]
    public class MultiPropertyCompositeWithPkVertex
    {
        [PropertyKey]
        [PrimaryKey]
        [CompositeIndex("multi_idx")]
        public string PkProp { get; set; } = string.Empty;

        [PropertyKey]
        [CompositeIndex("multi_idx")]
        public string OtherProp { get; set; } = string.Empty;
    }
}
