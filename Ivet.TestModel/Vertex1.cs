using Ivet.Model;

namespace Ivet.TestModel
{
    [Vertex]
    public class Vertex1
    {
        [PropertyKey]
        [PrimaryKey()]
        public string Id { get; set; }

        [EdgeProperty]
        public List<Vertex3> vertex3s { get; private set; } = new List<Vertex3>();

        [EdgeProperty]
        public Vertex3[] array_vertex3s { get; private set; }
    }
}