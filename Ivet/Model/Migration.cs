namespace Ivet.Model
{
    [Vertex]
    public class Migration
    {
        [PropertyKey()]
        [PrimaryKey()]
        public string MigrationName { get; set; }
        [PropertyKey()]
        public DateTime? MigrationDate { get; set; }
    }
}
