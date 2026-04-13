using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex]
    public class PrimaryKeyAndMultipleCompositeIndexVertex
    {
        [PropertyKey]
        [PrimaryKey]
        [CompositeIndex("first_redundant")]
        [CompositeIndex("second_redundant")]
        public string Id { get; set; } = string.Empty;
    }
}
