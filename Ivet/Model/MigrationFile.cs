namespace Ivet.Model
{
    public class MigrationFile
    {
        public string Description { get; set; } = string.Empty;
        public string? Content {  get; set; }
        public List<MigrationScript>? Scripts { get; set; }
    }
}
