using ConsoleTables;
using Ivet.Model.Meta;

namespace Ivet.Services
{
    public static class DeltaSchemaSummaryService
    {
        public static void Print(MetaSchema delta)
        {
            var total = 0;

            if (delta.Vertices.Count > 0)
            {
                var table = new ConsoleTable("Name", "Partitioned", "Static");
                delta.Vertices.ForEach(v => table.AddRow(v.Name, v.Partitioned, v.Static));
                Console.WriteLine("Vertices:");
                table.Write();
                total += delta.Vertices.Count;
            }

            if (delta.Edges.Count > 0)
            {
                var table = new ConsoleTable("Name", "Multiplicity");
                delta.Edges.ForEach(e => table.AddRow(e.Name, e.Multiplicity));
                Console.WriteLine("Edges:");
                table.Write();
                total += delta.Edges.Count;
            }

            if (delta.Properties.Count > 0)
            {
                var table = new ConsoleTable("Name", "Cardinality", "DataType");
                delta.Properties.ForEach(p => table.AddRow(p.Name, p.Cardinality, p.DataType));
                Console.WriteLine("Properties:");
                table.Write();
                total += delta.Properties.Count;
            }

            if (delta.Connections.Count > 0)
            {
                var table = new ConsoleTable("Edge", "Ingoing", "Outgoing");
                delta.Connections.ForEach(c => table.AddRow(c.Edge, c.Ingoing, c.Outgoing));
                Console.WriteLine("Connections:");
                table.Write();
                total += delta.Connections.Count;
            }

            if (delta.VertexPropertyBindings.Count > 0)
            {
                var table = new ConsoleTable("Property", "Entity");
                delta.VertexPropertyBindings.ForEach(b => table.AddRow(b.Name, b.Entity));
                Console.WriteLine("VertexPropertyBindings:");
                table.Write();
                total += delta.VertexPropertyBindings.Count;
            }

            if (delta.EdgePropertyBindings.Count > 0)
            {
                var table = new ConsoleTable("Property", "Entity");
                delta.EdgePropertyBindings.ForEach(b => table.AddRow(b.Name, b.Entity));
                Console.WriteLine("EdgePropertyBindings:");
                table.Write();
                total += delta.EdgePropertyBindings.Count;
            }

            if (delta.CompositeIndexes.Count > 0)
            {
                var table = new ConsoleTable("Name", "IsUnique", "IndexedElement");
                delta.CompositeIndexes.ForEach(i => table.AddRow(i.Name, i.IsUnique, i.IndexedElement));
                Console.WriteLine("CompositeIndexes:");
                table.Write();
                total += delta.CompositeIndexes.Count;
            }

            if (delta.MixedIndexes.Count > 0)
            {
                var table = new ConsoleTable("Name", "BackendIndex", "IndexedElement");
                delta.MixedIndexes.ForEach(i => table.AddRow(i.Name, i.BackendIndex, i.IndexedElement));
                Console.WriteLine("MixedIndexes:");
                table.Write();
                total += delta.MixedIndexes.Count;
            }

            if (delta.IndexBindings.Count > 0)
            {
                var table = new ConsoleTable("IndexName", "PropertyName", "Mapping");
                delta.IndexBindings.ForEach(b => table.AddRow(b.IndexName, b.PropertyName, b.Mapping));
                Console.WriteLine("IndexBindings:");
                table.Write();
                total += delta.IndexBindings.Count;
            }

            if (total == 0)
            {
                Console.WriteLine("No changes detected. Migration would be empty.");
            }
            else
            {
                Console.WriteLine($"Total: {total} element(s) to add.");
            }
        }
    }
}
