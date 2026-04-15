using Ivet.Model.Database;
using Ivet.Services;
using Ivet.Verbs.Services;
using Xunit;

namespace Ivet.Tests.Services
{
    public class StatusActionTests
    {
        [Fact]
        public void Render_NoRows_PrintsEmptyMessage()
        {
            var writer = new StringWriter();

            StatusAction.Render(Array.Empty<IndexStatusRow>(), writer);

            Assert.Contains("No indices found", writer.ToString());
        }

        [Fact]
        public void Render_SingleCompositeIndexEnabled_NoStuckMarker()
        {
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "brand_asciiName", IndexType = "composite", PropertyName = "AsciiName", DataType = "String", Cardinality = "SINGLE", Status = "ENABLED" }
            };
            var writer = new StringWriter();

            StatusAction.Render(rows, writer);
            var output = writer.ToString();

            Assert.Contains("brand_asciiName (composite, ENABLED)", output);
            Assert.Contains("AsciiName", output);
            Assert.DoesNotContain("stuck", output);
        }

        [Fact]
        public void Render_MixedIndexWithStuckKey_MarksItAndShowsMixedSummary()
        {
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "search", IndexType = "mixed", PropertyName = "Name", DataType = "String", Cardinality = "SINGLE", Status = "ENABLED" },
                new IndexStatusRow { IndexName = "search", IndexType = "mixed", PropertyName = "VisibleToOrgs", DataType = "String", Cardinality = "SET", Status = "INSTALLED" }
            };
            var writer = new StringWriter();

            StatusAction.Render(rows, writer);
            var output = writer.ToString();

            Assert.Contains("search (mixed,", output);
            Assert.Contains("ENABLED", output);
            Assert.Contains("INSTALLED", output);
            Assert.Contains("stuck", output);
        }

        [Fact]
        public void Render_MultipleIndexes_GroupedAndSortedAlphabetically()
        {
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "zeta", IndexType = "composite", PropertyName = "A", DataType = "String", Cardinality = "SINGLE", Status = "ENABLED" },
                new IndexStatusRow { IndexName = "alpha", IndexType = "composite", PropertyName = "B", DataType = "String", Cardinality = "SINGLE", Status = "ENABLED" }
            };
            var writer = new StringWriter();

            StatusAction.Render(rows, writer);
            var output = writer.ToString();

            Assert.True(output.IndexOf("alpha", StringComparison.Ordinal) < output.IndexOf("zeta", StringComparison.Ordinal));
        }

        [Fact]
        public void Render_AllKeysEnabled_SummaryShowsEnabled()
        {
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "idx", IndexType = "composite", PropertyName = "A", DataType = "String", Cardinality = "SINGLE", Status = "ENABLED" },
                new IndexStatusRow { IndexName = "idx", IndexType = "composite", PropertyName = "B", DataType = "String", Cardinality = "SINGLE", Status = "ENABLED" }
            };
            var writer = new StringWriter();

            StatusAction.Render(rows, writer);
            var output = writer.ToString();

            Assert.Contains("idx (composite, ENABLED)", output);
        }

        [Fact]
        public void Render_MultipleKeysSameIndex_EachKeyPrintedOnOwnLine()
        {
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "idx", IndexType = "composite", PropertyName = "FirstName", DataType = "String", Cardinality = "SINGLE", Status = "ENABLED" },
                new IndexStatusRow { IndexName = "idx", IndexType = "composite", PropertyName = "LastName", DataType = "String", Cardinality = "SINGLE", Status = "ENABLED" }
            };
            var writer = new StringWriter();

            StatusAction.Render(rows, writer);
            var lines = writer.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            Assert.Equal(3, lines.Length);
            Assert.StartsWith("idx", lines[0]);
            Assert.Contains("FirstName", lines[1]);
            Assert.Contains("LastName", lines[2]);
        }

        [Fact]
        public void Render_KeysOrderedAlphabetically_WithinIndex()
        {
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "idx", IndexType = "composite", PropertyName = "Zebra", DataType = "String", Cardinality = "SINGLE", Status = "ENABLED" },
                new IndexStatusRow { IndexName = "idx", IndexType = "composite", PropertyName = "Apple", DataType = "String", Cardinality = "SINGLE", Status = "ENABLED" }
            };
            var writer = new StringWriter();

            StatusAction.Render(rows, writer);
            var output = writer.ToString();

            Assert.True(output.IndexOf("Apple", StringComparison.Ordinal) < output.IndexOf("Zebra", StringComparison.Ordinal));
        }

        [Fact]
        public void Render_NonEnabledStatusLowercase_StillMarkedAsStuck()
        {
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "idx", IndexType = "composite", PropertyName = "A", DataType = "String", Cardinality = "SINGLE", Status = "REGISTERED" }
            };
            var writer = new StringWriter();

            StatusAction.Render(rows, writer);

            Assert.Contains("stuck", writer.ToString());
        }

        [Fact]
        public void Render_EnabledStatusDifferentCase_NotMarkedAsStuck()
        {
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "idx", IndexType = "composite", PropertyName = "A", DataType = "String", Cardinality = "SINGLE", Status = "enabled" }
            };
            var writer = new StringWriter();

            StatusAction.Render(rows, writer);

            Assert.DoesNotContain("stuck", writer.ToString());
        }

        [Fact]
        public void Render_ColumnsAligned_PaddedToLongestValue()
        {
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "idx", IndexType = "composite", PropertyName = "Short", DataType = "String", Cardinality = "SINGLE", Status = "ENABLED" },
                new IndexStatusRow { IndexName = "idx", IndexType = "composite", PropertyName = "MuchLongerPropertyName", DataType = "String", Cardinality = "SINGLE", Status = "ENABLED" }
            };
            var writer = new StringWriter();

            StatusAction.Render(rows, writer);
            var lines = writer.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            var shortLine = lines.First(l => l.Contains("Short ")); // padding adds spaces
            Assert.Contains("Short                  ", shortLine);
        }

        [Fact]
        public void Render_MixedStatusesAcrossKeys_SummaryListsAllDistinct()
        {
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "idx", IndexType = "mixed", PropertyName = "A", DataType = "String", Cardinality = "SINGLE", Status = "ENABLED" },
                new IndexStatusRow { IndexName = "idx", IndexType = "mixed", PropertyName = "B", DataType = "String", Cardinality = "SINGLE", Status = "INSTALLED" },
                new IndexStatusRow { IndexName = "idx", IndexType = "mixed", PropertyName = "C", DataType = "String", Cardinality = "SINGLE", Status = "REGISTERED" }
            };
            var writer = new StringWriter();

            StatusAction.Render(rows, writer);
            var output = writer.ToString();

            Assert.Matches(@"idx \(mixed, mixed \(.*ENABLED.*\)\)", output);
            Assert.Contains("INSTALLED", output);
            Assert.Contains("REGISTERED", output);
        }

        [Fact]
        public void Render_SingleIndexNoKeys_NotSupportedByGroupingContract()
        {
            // Guard: groups are built from rows, so an index with zero keys never appears.
            // A DB-generated CSV always has at least one row per index (getFieldKeys is non-empty).
            var rows = Array.Empty<IndexStatusRow>();
            var writer = new StringWriter();

            StatusAction.Render(rows, writer);

            Assert.DoesNotContain("(composite", writer.ToString());
            Assert.DoesNotContain("(mixed", writer.ToString());
        }

        [Fact]
        public void Parser_GetIndexStatusRows_ParsesCsvCorrectly()
        {
            var csv = "|IndexName|IndexType|IsUnique|PropertyName|DataType|Cardinality|Status|\n" +
                      "|search|mixed|False|Name|String|SINGLE|ENABLED|\n" +
                      "|search|mixed|False|VisibleToOrgs|String|SET|INSTALLED|\n";

            var rows = new Parser().GetIndexStatusRows(csv);

            Assert.Equal(2, rows.Count);
            Assert.Equal("search", rows[0].IndexName);
            Assert.Equal("mixed", rows[0].IndexType);
            Assert.False(rows[0].IsUnique);
            Assert.Equal("Name", rows[0].PropertyName);
            Assert.Equal("ENABLED", rows[0].Status);
            Assert.Equal("INSTALLED", rows[1].Status);
            Assert.Equal("SET", rows[1].Cardinality);
        }

        [Fact]
        public void Parser_GetIndexStatusRows_EmptyCsv_ReturnsEmptyList()
        {
            var csv = "|IndexName|IndexType|IsUnique|PropertyName|DataType|Cardinality|Status|\n";

            var rows = new Parser().GetIndexStatusRows(csv);

            Assert.Empty(rows);
        }

        [Fact]
        public void Parser_GetIndexStatusRows_UniqueCompositeIndex_ParsesIsUniqueTrue()
        {
            var csv = "|IndexName|IndexType|IsUnique|PropertyName|DataType|Cardinality|Status|\n" +
                      "|Migration_PK|composite|True|MigrationName|String|SINGLE|ENABLED|\n";

            var rows = new Parser().GetIndexStatusRows(csv);

            Assert.Single(rows);
            Assert.True(rows[0].IsUnique);
            Assert.Equal("composite", rows[0].IndexType);
        }
    }
}
