namespace Ivet.Model.Meta
{
    public class MetaSchema
    {
        public List<MetaVertex> Vertices { get; private set; } = new List<MetaVertex> { };
        public List<MetaPropertyKey> Properties { get; private set; } = new List<MetaPropertyKey> { };
        public List<MetaEdge> Edges { get; private set; } = new List<MetaEdge> { };
        public List<MetaConnection> Connections { get; private set; } = new List<MetaConnection> { };
        public List<MetaPropertyBinding> VertexPropertyBindings { get; private set; } = new List<MetaPropertyBinding> { };
        public List<MetaPropertyBinding> EdgePropertyBindings { get; private set; } = new List<MetaPropertyBinding> { };
        public List<MetaCompositeIndex> CompositeIndexes { get; private set; } = new List<MetaCompositeIndex>();
        public List<MetaMixedIndex> MixedIndexes { get; private set; } = new List<MetaMixedIndex>();
        public List<MetaIndexBinding> IndexBindings { get; private set; } = new List<MetaIndexBinding>();
    }
}
