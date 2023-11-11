namespace Ivet.Model.Meta
{
    public class MetaCompositeIndex
    {
        public string Name { get; set; } = string.Empty;
        public bool IsUnique { get; set; }
        public string IndexedElement { get; set; } = string.Empty;
        public string Kind {  get; set; } = string.Empty;
    }
}
