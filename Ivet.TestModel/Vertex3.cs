using Ivet.Model;

namespace Ivet.TestModel1
{
    [Vertex]
    public class Vertex3
    {
        [PropertyKey]
        [CompositeIndex("vertex3_composite")]
        public string? Name { get; set; }
    }
}
