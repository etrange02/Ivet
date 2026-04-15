namespace Ivet.Model.Database
{
    public class IndexStatusRow
    {
        public string IndexName { get; set; } = string.Empty;
        public string IndexType { get; set; } = string.Empty;
        public bool IsUnique { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string Cardinality { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
