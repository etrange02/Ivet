using CsvHelper.Configuration.Attributes;

namespace Ivet.Model.Database
{
    public class PropertyKey
    {
        [Name("Property Key Name")]
        public string Name { get; set; }
        public Cardinality Cardinality { get; set; }
        [Name("Data Type")]
        public string DataType { get; set; }
    }
}
