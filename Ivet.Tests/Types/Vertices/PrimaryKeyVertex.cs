using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    [Vertex]
    public class PrimaryKeyVertex
    {
        [PropertyKey]
        [PrimaryKey]
        public string Id { get; set; } = string.Empty;
    }
}
