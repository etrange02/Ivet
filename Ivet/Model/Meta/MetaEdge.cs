namespace Ivet.Model.Meta
{
    public class MetaEdge : AbstractMetaItem<EdgeAttribute>
    {
        public Multiplicity Multiplicity { get; set; }
        public Type? In { get; set; }
        public Type? Out { get; set; }
    }
}
