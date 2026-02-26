using Ivet.Model.Meta;

namespace Ivet.Services
{
    public static class SchemaWarningService
    {
        public static void PrintRemovals(MetaSchema removals)
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

            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine();
            Console.WriteLine("Warning: The following elements exist in the database but not in the code:");
            foreach (var (category, names) in sections)
            {
                Console.WriteLine($"  {category}:");
                foreach (var name in names)
                    Console.WriteLine($"    - {name}");
            }
            Console.WriteLine("These elements will NOT be removed (JanusGraph does not support schema deletion).");
            Console.WriteLine();

            Console.ForegroundColor = previousColor;
        }

        public static void PrintModifications(List<MetaSchemaModification> modifications)
        {
            if (modifications.Count == 0)
                return;

            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine();
            Console.WriteLine("Warning: The following schema elements have been modified:");
            foreach (var group in modifications.GroupBy(m => m.ElementType))
            {
                Console.WriteLine($"  {group.Key}:");
                foreach (var mod in group)
                    Console.WriteLine($"    - {mod.ElementName}.{mod.PropertyName}: {mod.SourceValue} -> {mod.TargetValue}");
            }
            Console.WriteLine("JanusGraph does not support modifying existing schema elements.");
            Console.WriteLine();

            Console.ForegroundColor = previousColor;
        }
    }
}
