using Ivet.Model;

namespace Ivet.TestModel1
{
    [Edge(typeof(Vertex1), typeof(Vertex2))]
    public class EdgeWithProperties
    {
        [PropertyKey]
        public int Quantity { get; set; }
    }
}
