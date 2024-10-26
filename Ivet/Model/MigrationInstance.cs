namespace Ivet.Model
{
    internal class MigrationInstance
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Script { get; set; }
        public bool IsMulti { get; set; }
    }
}
