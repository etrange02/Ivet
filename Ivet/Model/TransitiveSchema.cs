using Ivet.Model.Meta;

namespace Ivet.Model
{
    public class TransitiveSchema
    {
        public MetaSchema? Source { get; set; }
        public MetaSchema? Target { get; set; }
        public MetaSchema? Difference { get; set; }
    }
}
