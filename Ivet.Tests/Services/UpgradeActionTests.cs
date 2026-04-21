using Ivet.Model.Database;
using Ivet.TestFramework;
using Ivet.Verbs.Services;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Ivet.Tests.Services
{
    public class UpgradeActionTests
    {
        [Fact]
        public void GetNonEnabledRows_AllEnabled_ReturnsEmpty()
        {
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "idx", Status = "ENABLED", PropertyName = "A" },
                new IndexStatusRow { IndexName = "idx", Status = "ENABLED", PropertyName = "B" }
            };

            var result = StatusAction.GetNonEnabledRows(rows);

            Assert.Empty(result);
        }

        [Fact]
        public void GetNonEnabledRows_SomeNonEnabled_ReturnsOnlyNonEnabled()
        {
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "idx", Status = "ENABLED", PropertyName = "A" },
                new IndexStatusRow { IndexName = "idx", Status = "INSTALLED", PropertyName = "B" },
                new IndexStatusRow { IndexName = "idx", Status = "REGISTERED", PropertyName = "C" }
            };

            var result = StatusAction.GetNonEnabledRows(rows);

            Assert.Equal(2, result.Count);
            Assert.DoesNotContain(result, r => r.Status == "ENABLED");
        }

        [Fact]
        public void GetNonEnabledRows_LowercaseEnabled_TreatedAsEnabled()
        {
            var rows = new[] { new IndexStatusRow { IndexName = "idx", Status = "enabled", PropertyName = "A" } };

            var result = StatusAction.GetNonEnabledRows(rows);

            Assert.Empty(result);
        }

        [Fact]
        public void GetNonEnabledRows_EmptyInput_ReturnsEmpty()
        {
            var result = StatusAction.GetNonEnabledRows(Array.Empty<IndexStatusRow>());

            Assert.Empty(result);
        }

        [Fact]
        public void VerifyIndexStatuses_AllEnabled_ReturnsTrueAndLogsInfo()
        {
            Environment.ExitCode = 0;
            var rows = new[] { new IndexStatusRow { IndexName = "idx", Status = "ENABLED", PropertyName = "A" } };
            var logger = new TestLogger();

            var ok = UpgradeAction.VerifyIndexStatuses(rows, logger);

            Assert.True(ok);
            Assert.Equal(0, Environment.ExitCode);
            Assert.Contains(logger.Entries, e => e.Level == LogLevel.Information && e.Message.Contains("all indices ENABLED"));
        }

        [Fact]
        public void VerifyIndexStatuses_OneStuck_ReturnsFalseSetsExitCodeAndLogsError()
        {
            Environment.ExitCode = 0;
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "idx", Status = "ENABLED", PropertyName = "A" },
                new IndexStatusRow { IndexName = "search", Status = "INSTALLED", PropertyName = "VisibleToOrgs" }
            };
            var logger = new TestLogger();

            var ok = UpgradeAction.VerifyIndexStatuses(rows, logger);

            Assert.False(ok);
            Assert.Equal(1, Environment.ExitCode);
            var errorEntry = logger.Entries.Single(e => e.Level == LogLevel.Error);
            Assert.Contains("search.VisibleToOrgs=INSTALLED", errorEntry.Message);
            Assert.Contains("1 non-ENABLED", errorEntry.Message);
            Environment.ExitCode = 0;
        }

        [Fact]
        public void VerifyIndexStatuses_MultipleStuck_ListsAllInError()
        {
            Environment.ExitCode = 0;
            var rows = new[]
            {
                new IndexStatusRow { IndexName = "a", Status = "INSTALLED", PropertyName = "x" },
                new IndexStatusRow { IndexName = "b", Status = "REGISTERED", PropertyName = "y" },
                new IndexStatusRow { IndexName = "c", Status = "ENABLED", PropertyName = "z" }
            };
            var logger = new TestLogger();

            var ok = UpgradeAction.VerifyIndexStatuses(rows, logger);

            Assert.False(ok);
            var msg = logger.Entries.Single(e => e.Level == LogLevel.Error).Message;
            Assert.Contains("a.x=INSTALLED", msg);
            Assert.Contains("b.y=REGISTERED", msg);
            Assert.DoesNotContain("c.z", msg);
            Assert.Contains("2 non-ENABLED", msg);
            Environment.ExitCode = 0;
        }

        [Fact]
        public void VerifyIndexStatuses_EmptyRows_ReturnsTrue()
        {
            Environment.ExitCode = 0;
            var logger = new TestLogger();

            var ok = UpgradeAction.VerifyIndexStatuses(Array.Empty<IndexStatusRow>(), logger);

            Assert.True(ok);
            Assert.Equal(0, Environment.ExitCode);
        }

        [Fact]
        public void VerifyIndexStatuses_Success_DoesNotTouchExitCode()
        {
            Environment.ExitCode = 42;
            var rows = new[] { new IndexStatusRow { IndexName = "idx", Status = "ENABLED", PropertyName = "A" } };
            var logger = new TestLogger();

            UpgradeAction.VerifyIndexStatuses(rows, logger);

            Assert.Equal(42, Environment.ExitCode);
            Environment.ExitCode = 0;
        }
    }
}
