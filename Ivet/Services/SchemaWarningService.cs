using Ivet.Model.Meta;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Ivet.Services
{
    public static class SchemaWarningService
    {
        public static void PrintRemovals(MetaSchema removals, ILogger logger)
        {
            var sections = new List<(string Category, List<string> Names)>();

            if (removals.Vertices.Count > 0)
                sections.Add(("Vertices", removals.Vertices.Select(v => v.Name).ToList()));
            if (removals.Edges.Count > 0)
                sections.Add(("Edges", removals.Edges.Select(e => e.Name).ToList()));
            if (removals.Properties.Count > 0)
                sections.Add(("Properties", removals.Properties.Select(p => p.Name).ToList()));
            if (removals.Connections.Count > 0)
                sections.Add(("Connections", removals.Connections.Select(c => $"{c.Edge} ({c.Outgoing} -> {c.Ingoing})").ToList()));
            if (removals.VertexPropertyBindings.Count > 0)
                sections.Add(("VertexPropertyBindings", removals.VertexPropertyBindings.Select(b => $"{b.Name} on {b.Entity}").ToList()));
            if (removals.EdgePropertyBindings.Count > 0)
                sections.Add(("EdgePropertyBindings", removals.EdgePropertyBindings.Select(b => $"{b.Name} on {b.Entity}").ToList()));
            if (removals.CompositeIndexes.Count > 0)
                sections.Add(("CompositeIndexes", removals.CompositeIndexes.Select(i => i.Name).ToList()));
            if (removals.MixedIndexes.Count > 0)
                sections.Add(("MixedIndexes", removals.MixedIndexes.Select(i => i.Name).ToList()));
            if (removals.IndexBindings.Count > 0)
                sections.Add(("IndexBindings", removals.IndexBindings.Select(b => $"{b.IndexName}.{b.PropertyName}").ToList()));

            if (sections.Count == 0)
                return;

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("Warning: The following elements exist in the database but not in the code:");
            foreach (var (category, names) in sections)
            {
                sb.AppendLine($"  {category}:");
                foreach (var name in names)
                    sb.AppendLine($"    - {name}");
            }
            sb.Append("These elements will NOT be removed (JanusGraph does not support schema deletion).");
            logger.LogWarning("{Message}", sb.ToString());
        }

        public static void PrintModifications(List<MetaSchemaModification> modifications, ILogger logger)
        {
            if (modifications.Count == 0)
                return;

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("Warning: The following schema elements have been modified:");
            foreach (var group in modifications.GroupBy(m => m.ElementType))
            {
                sb.AppendLine($"  {group.Key}:");
                foreach (var mod in group)
                    sb.AppendLine($"    - {mod.ElementName}.{mod.PropertyName}: {mod.SourceValue} -> {mod.TargetValue}");
            }
            sb.Append("JanusGraph does not support modifying existing schema elements.");
            logger.LogWarning("{Message}", sb.ToString());
        }
    }
}
