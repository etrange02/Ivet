namespace Ivet.Model.Database
{
    public class Index
    {
        public string Name { get; set; } = string.Empty;
        public bool IsUnique { get; set; }
        public bool IsMixedIndex { get; set; }
        public bool IsCompositeIndex { get; set; }
        public string BackendIndex { get; set; } = string.Empty;
        public string IndexedElement {  get; set; } = string.Empty;
    }
}
