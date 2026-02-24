namespace Ivet.Model
{
    public class MigrationScript
    {
        public required string Script { get; set; }
        public long? EvaluationTimeout { get; set; }
    }
}
