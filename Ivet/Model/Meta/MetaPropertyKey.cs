using System.Reflection;

namespace Ivet.Model.Meta
{
    public class MetaPropertyKey
    {
        public string Name { get; set; } = string.Empty;
        public Cardinality Cardinality { get; set; }
        public string? DataType { get; set; }
        public PropertyInfo? PropertyInfo { get; set; }
    }
}
