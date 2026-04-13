using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex]
    public class PrimaryKeyAndCompositeIndexVertex
    {
        [PropertyKey]
        [PrimaryKey]
        [CompositeIndex("redundant_idx")]
        public string Id { get; set; } = string.Empty;
    }
}
