using Ivet.Model.Database;
using Ivet.TestFramework;
using Ivet.Verbs.Services;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Ivet.Tests.Services
{
    public class ReindexActionTests
    {
        [Fact]
        public void ResolveTargets_NoIndexName_ReturnsOnlyNonEnabledIndices()
        {
            var statuses = new[]
            {
                new IndexStatusRow { IndexName = "enabled_idx", Status = "ENABLED", PropertyName = "A" },
                new IndexStatusRow { IndexName = "stuck_idx", Status = "INSTALLED", PropertyName = "B" },
                new IndexStatusRow { IndexName = "partial_idx", Status = "ENABLED", PropertyName = "C" },
                new IndexStatusRow { IndexName = "partial_idx", Status = "REGISTERED", PropertyName = "D" }
            };
            var logger = new TestLogger();

            var targets = ReindexAction.ResolveTargets(statuses, null, logger);

            Assert.Equal(new[] { "partial_idx", "stuck_idx" }, targets);
        }

        [Fact]
        public void ResolveTargets_NoIndexName_AllEnabled_ReturnsEmptyAndLogs()
        {
            var statuses = new[]
            {
                new IndexStatusRow { IndexName = "idx1", Status = "ENABLED", PropertyName = "A" },
                new IndexStatusRow { IndexName = "idx2", Status = "ENABLED", PropertyName = "B" }
            };
            var logger = new TestLogger();

            var targets = ReindexAction.ResolveTargets(statuses, null, logger);

            Assert.Empty(targets);
            Assert.Contains(logger.Entries, e => e.Message.Contains("All indices are ENABLED"));
        }

        [Fact]
        public void ResolveTargets_SpecificIndex_NotFound_SetsExitCodeAndReturnsEmpty()
        {
            Environment.ExitCode = 0;
            var statuses = new[] { new IndexStatusRow { IndexName = "idx1", Status = "ENABLED", PropertyName = "A" } };
            var logger = new TestLogger();

            var targets = ReindexAction.ResolveTargets(statuses, "missing", logger);

            Assert.Empty(targets);
            Assert.Equal(1, Environment.ExitCode);
            Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error && e.Message.Contains("missing"));
            Environment.ExitCode = 0;
        }

        [Fact]
        public void ResolveTargets_SpecificIndex_AlreadyEnabled_ReturnsEmptyAndLogsInfo()
        {
            var statuses = new[]
            {
                new IndexStatusRow { IndexName = "idx1", Status = "ENABLED", PropertyName = "A" },
                new IndexStatusRow { IndexName = "idx1", Status = "ENABLED", PropertyName = "B" }
            };
            var logger = new TestLogger();

            var targets = ReindexAction.ResolveTargets(statuses, "idx1", logger);

            Assert.Empty(targets);
            Assert.Contains(logger.Entries, e => e.Message.Contains("already ENABLED"));
        }

        [Fact]
        public void ResolveTargets_SpecificIndex_HasNonEnabledKey_ReturnsIt()
        {
            var statuses = new[]
            {
                new IndexStatusRow { IndexName = "idx1", Status = "ENABLED", PropertyName = "A" },
                new IndexStatusRow { IndexName = "idx1", Status = "INSTALLED", PropertyName = "B" }
            };
            var logger = new TestLogger();

            var targets = ReindexAction.ResolveTargets(statuses, "idx1", logger);

            Assert.Equal(new[] { "idx1" }, targets);
        }

        [Fact]
        public void ResolveTargets_NoIndexName_GroupedByIndexName_NotByKey()
        {
            var statuses = new[]
            {
                new IndexStatusRow { IndexName = "multi", Status = "INSTALLED", PropertyName = "A" },
                new IndexStatusRow { IndexName = "multi", Status = "INSTALLED", PropertyName = "B" },
                new IndexStatusRow { IndexName = "multi", Status = "INSTALLED", PropertyName = "C" }
            };
            var logger = new TestLogger();

            var targets = ReindexAction.ResolveTargets(statuses, null, logger);

            Assert.Equal(new[] { "multi" }, targets);
        }

        [Fact]
        public void BuildReindexScript_IncludesRegisterAndReindex()
        {
            var script = ReindexAction.BuildReindexScript("search", 60, false);

            Assert.Contains("SchemaAction.REGISTER_INDEX", script);
            Assert.Contains("SchemaAction.REINDEX", script);
            Assert.Contains("'search'", script);
            Assert.Contains("SchemaStatus.REGISTERED", script);
            Assert.Contains(".timeout(60, java.time.temporal.ChronoUnit.SECONDS)", script);
        }

        [Fact]
        public void BuildReindexScript_AwaitEnabledTrue_IncludesEnabledAwait()
        {
            var script = ReindexAction.BuildReindexScript("search", 60, true);

            Assert.Contains("SchemaStatus.ENABLED", script);
        }

        [Fact]
        public void BuildReindexScript_AwaitEnabledFalse_OmitsEnabledAwait()
        {
            var script = ReindexAction.BuildReindexScript("search", 60, false);

            Assert.DoesNotContain(".status(SchemaStatus.ENABLED)", script);
        }

        [Fact]
        public void BuildReindexScript_IdempotentGuard_SkipsIfAllEnabled()
        {
            var script = ReindexAction.BuildReindexScript("search", 60, false);

            Assert.Contains("allEnabled", script);
            Assert.Contains("if (!allEnabled)", script);
        }

        [Fact]
        public void BuildReindexScript_InvalidIndexName_Throws()
        {
            Assert.Throws<ArgumentException>(() => ReindexAction.BuildReindexScript("bad'name", 60, false));
        }

        [Fact]
        public void BuildReindexScript_TimeoutReflectedInBothAwaits()
        {
            var script = ReindexAction.BuildReindexScript("search", 120, true);

            var count = System.Text.RegularExpressions.Regex.Matches(script, @"\.timeout\(120,").Count;
            Assert.Equal(2, count);
        }
    }
}
