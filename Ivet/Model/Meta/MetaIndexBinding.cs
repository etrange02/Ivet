namespace Ivet.Model.Meta
{
    public class MetaIndexBinding
    {
        public string IndexName { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;
        public MappingType Mapping { get; set; }
    }
}
