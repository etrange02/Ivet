namespace Ivet.Services
{
    /// <summary>
    /// A migration already recorded in the graph (the <c>Migration</c> tracking vertex).
    /// Read back via raw Gremlin so ivet carries no ExRam.Gremlinq dependency.
    /// </summary>
    public record AppliedMigration(string Name, DateTime? Date);
}
