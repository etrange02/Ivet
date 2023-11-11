using Ivet.Model;

namespace Ivet.TestModel
{
    [Vertex]
    public class Vertex2
    {
        [PropertyKey]
        [MixedIndex("vertex2_mixed", Backend = "search", Mapping = MappingType.STRING)]
        public string Id { get; set; }
    }
}
