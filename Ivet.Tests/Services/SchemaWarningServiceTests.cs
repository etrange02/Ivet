using Ivet.Model.Meta;
using Ivet.Services;
using Ivet.TestFramework;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Ivet.Tests.Services
{
    public class SchemaWarningServiceTests
    {
        [Fact]
        public void PrintRemovals_EmptySchema_NoOutput()
        {
            var removals = new MetaSchema();
            var logger = new TestLogger();

            SchemaWarningService.PrintRemovals(removals, logger);

            Assert.Empty(logger.Entries);
        }

        [Fact]
        public void PrintRemovals_WithRemovals_PrintsWarning()
        {
            var removals = new MetaSchema();
            removals.Vertices.Add(new MetaVertex { Name = "TestVertex" });
            var logger = new TestLogger();

            SchemaWarningService.PrintRemovals(removals, logger);

            Assert.Single(logger.Entries);
            Assert.Equal(LogLevel.Warning, logger.Entries[0].Level);
            Assert.Contains("Warning", logger.Entries[0].Message);
            Assert.Contains("Vertices", logger.Entries[0].Message);
            Assert.Contains("TestVertex", logger.Entries[0].Message);
            Assert.Contains("will NOT be removed", logger.Entries[0].Message);
        }

        [Fact]
        public void PrintRemovals_MultipleCategories_PrintsAllCategories()
        {
            var removals = new MetaSchema();
            removals.Vertices.Add(new MetaVertex { Name = "V1" });
            removals.Edges.Add(new MetaEdge { Name = "E1" });
            removals.Properties.Add(new MetaPropertyKey { Name = "P1" });
            var logger = new TestLogger();

            SchemaWarningService.PrintRemovals(removals, logger);

            Assert.Single(logger.Entries);
            Assert.Equal(LogLevel.Warning, logger.Entries[0].Level);
            Assert.Contains("Vertices", logger.Entries[0].Message);
            Assert.Contains("Edges", logger.Entries[0].Message);
            Assert.Contains("Properties", logger.Entries[0].Message);
        }

        [Fact]
        public void PrintModifications_EmptyList_NoOutput()
        {
            var logger = new TestLogger();

            SchemaWarningService.PrintModifications(new List<MetaSchemaModification>(), logger);

            Assert.Empty(logger.Entries);
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
            var logger = new TestLogger();

            SchemaWarningService.PrintModifications(modifications, logger);

            Assert.Single(logger.Entries);
            Assert.Equal(LogLevel.Warning, logger.Entries[0].Level);
            Assert.Contains("Warning", logger.Entries[0].Message);
            Assert.Contains("Vertex", logger.Entries[0].Message);
            Assert.Contains("MyVertex.Partitioned", logger.Entries[0].Message);
            Assert.Contains("False -> True", logger.Entries[0].Message);
            Assert.Contains("does not support modifying", logger.Entries[0].Message);
        }
    }
}
