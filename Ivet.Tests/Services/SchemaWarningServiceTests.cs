using Ivet.Model.Meta;
using Ivet.Services;
using Ivet.TestFramework;
using Xunit;

namespace Ivet.Tests.Services
{
    public class SchemaWarningServiceTests
    {
        [Fact]
        public void PrintRemovals_EmptySchema_NoOutput()
        {
            var removals = new MetaSchema();
            var writer = new StringWriter();
            Console.SetOut(writer);

            SchemaWarningService.PrintRemovals(removals);

            Assert.Empty(writer.ToString());
        }

        [Fact]
        public void PrintRemovals_WithRemovals_PrintsWarning()
        {
            var removals = new MetaSchema();
            removals.Vertices.Add(new MetaVertex { Name = "TestVertex" });
            var writer = new StringWriter();
            Console.SetOut(writer);

            SchemaWarningService.PrintRemovals(removals);

            var output = writer.ToString();
            Assert.Contains("Warning", output);
            Assert.Contains("Vertices", output);
            Assert.Contains("TestVertex", output);
            Assert.Contains("will NOT be removed", output);
        }

        [Fact]
        public void PrintRemovals_MultipleCategories_PrintsAllCategories()
        {
            var removals = new MetaSchema();
            removals.Vertices.Add(new MetaVertex { Name = "V1" });
            removals.Edges.Add(new MetaEdge { Name = "E1" });
            removals.Properties.Add(new MetaPropertyKey { Name = "P1" });
            var writer = new StringWriter();
            Console.SetOut(writer);

            SchemaWarningService.PrintRemovals(removals);

            var output = writer.ToString();
            Assert.Contains("Vertices", output);
            Assert.Contains("Edges", output);
            Assert.Contains("Properties", output);
        }
        [Fact]
        public void PrintModifications_EmptyList_NoOutput()
        {
            var writer = new StringWriter();
            Console.SetOut(writer);

            SchemaWarningService.PrintModifications(new List<MetaSchemaModification>());

            Assert.Empty(writer.ToString());
        }

        [Fact]
        public void PrintModifications_WithModifications_PrintsWarning()
        {
            var modifications = new List<MetaSchemaModification>
            {
                new MetaSchemaModification
                {
                    ElementType = "Vertex",
                    ElementName = "MyVertex",
                    PropertyName = "Partitioned",
                    SourceValue = "False",
                    TargetValue = "True"
                }
            };
            var writer = new StringWriter();
            Console.SetOut(writer);

            SchemaWarningService.PrintModifications(modifications);

            var output = writer.ToString();
            Assert.Contains("Warning", output);
            Assert.Contains("Vertex", output);
            Assert.Contains("MyVertex.Partitioned", output);
            Assert.Contains("False -> True", output);
            Assert.Contains("does not support modifying", output);
        }
    }
}
