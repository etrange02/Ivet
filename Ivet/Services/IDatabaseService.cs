namespace Ivet.Services
{
    public interface IDatabaseService
    {
        IReadOnlyList<AppliedMigration> GetAppliedMigrations(IEnumerable<string> names);
        void AddAppliedMigration(string name, DateTime date);

        string GetConnectionSchema();
        string GetEdgeSchema();
        string GetEdgesPropertyBindingsSchema();
        string GetIndexBindingSchema();
        string GetIndexSchema();
        string GetIndexStatusSchema();
        string GetPropertyKeysSchema();
        string GetVertexPropertyBindingsSchema();
        string GetVertexSchema();
    }
}
