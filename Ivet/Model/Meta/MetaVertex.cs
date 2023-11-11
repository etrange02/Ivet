namespace Ivet.Model.Meta
{
    public class MetaVertex : AbstractMetaItem<VertexAttribute>
    {
        public bool Partitioned { get; set; }
        public bool Static { get; set; }
    }
}
