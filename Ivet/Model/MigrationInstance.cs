namespace Ivet.Model
{
    internal class MigrationInstance
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Script { get; set; }
        public required string RelativePath { get; set; }
        public bool IsMulti { get; set; }
    }
}
