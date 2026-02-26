using Ivet.Model;
using Ivet.Model.Meta;
using Ivet.Services;
using Ivet.TestFramework;
using Xunit;

namespace Ivet.Tests.Services
{
    public class DeltaSchemaSummaryServiceTests
    {
        [Fact]
        public void Print_EmptySchema_PrintsNoChanges()
        {
            var delta = new MetaSchema();
            var writer = new StringWriter();
            Console.SetOut(writer);

            DeltaSchemaSummaryService.Print(delta);

            var output = writer.ToString();
            Assert.Contains("No changes detected", output);
        }

        [Fact]
        public void Print_WithVertices_PrintsVertexSection()
        {
            var delta = new MetaSchema();
            delta.Vertices.Add(new MetaVertex { Name = "TestVertex", Partitioned = true, Static = false });
            var writer = new StringWriter();
            Console.SetOut(writer);

            DeltaSchemaSummaryService.Print(delta);

            var output = writer.ToString();
            Assert.Contains("Vertices", output);
            Assert.Contains("TestVertex", output);
            Assert.Contains("Total: 1 element(s) to add.", output);
        }

        [Fact]
        public void Print_AllCategories_PrintsCorrectTotal()
        {
            var delta = new MetaSchema();
            delta.Vertices.Add(new MetaVertex { Name = "V1" });
            delta.Edges.Add(new MetaEdge { Name = "E1" });
            delta.Properties.Add(new MetaPropertyKey { Name = "P1" });
            delta.Connections.Add(new MetaConnection { Edge = "E1", Ingoing = "V1", Outgoing = "V1" });
            delta.VertexPropertyBindings.Add(new MetaPropertyBinding { Name = "P1", Entity = "V1" });
            delta.EdgePropertyBindings.Add(new MetaPropertyBinding { Name = "P1", Entity = "E1" });
            delta.CompositeIndexes.Add(new MetaCompositeIndex { Name = "CI1" });
            delta.MixedIndexes.Add(new MetaMixedIndex { Name = "MI1" });
            delta.IndexBindings.Add(new MetaIndexBinding { IndexName = "CI1", PropertyName = "P1" });
            var writer = new StringWriter();
            Console.SetOut(writer);

            DeltaSchemaSummaryService.Print(delta);

            var output = writer.ToString();
            Assert.Contains("Total: 9 element(s) to add.", output);
        }
    }
}
