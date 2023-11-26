namespace Ivet.Model.Database
{
    public class Schema
    {
        public List<Vertex> Vertices { get; private set; } = new List<Vertex> { };
        public List<Edge> Edges { get; private set; } = new List<Edge> { };
        public List<PropertyKey> PropertyKeys { get; private set; } = new List<PropertyKey> { };
        public List<Connection> Connections { get; private set; } = new List<Connection> { };
        public List<PropertyBinding> VertexPropertyBindings { get; private set; } = new List<PropertyBinding> { };
        public List<PropertyBinding> EdgesPropertyBindings { get; private set; } = new List<PropertyBinding> { };
        public List<Index> Indices { get; private set; } = new List<Index> { };
        public List<IndexBinding> IndexBindings { get; private set; } = new List<IndexBinding> { };
    }
}
