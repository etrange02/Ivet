namespace Ivet.Model
{
    [Vertex]
    public class Migration : AbstractVertex
    {
        [PropertyKey]
        [PrimaryKey]
        public string? MigrationName { get; set; }
        [PropertyKey]
        public DateTime? MigrationDate { get; set; }
    }
}
