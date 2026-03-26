namespace Ivet.Model
{
    [Vertex]
    public class Migration : AbstractVertex
    {
        [PropertyKey]
        [PrimaryKey]
        [CompositeIndex("ivet_migration_migrationName")]
        public string? MigrationName { get; set; }
        [PropertyKey]
        public DateTime? MigrationDate { get; set; }
    }
}
