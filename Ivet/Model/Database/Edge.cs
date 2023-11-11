using CsvHelper.Configuration.Attributes;

namespace Ivet.Model.Database
{
    public class Edge
    {
        [Name("Edge Label Name")]
        public string Name { get; set; } = string.Empty;
        public bool Directed { get; set; }
        public bool Unidirected { get; set; }
        public Multiplicity Multiplicity { get; set; }
    }
}
