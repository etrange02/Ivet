using CsvHelper.Configuration.Attributes;

namespace Ivet.Model.Database
{
    public class Vertex
    {
        [Name("Vertex Label Name")]
        public string Name { get; set; } = string.Empty;
        public bool Partitioned { get; set; } = false;
        public bool Static { get; set; } = false;
    }
}
