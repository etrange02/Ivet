using Ivet.Services.Comparers;
using Xunit;

namespace Ivet.Tests.Services.Comparers
{
    public class NaturalSortComparerTests
    {
        private readonly NaturalSortComparer _sut = NaturalSortComparer.Instance;

        [Theory]
        [InlineData("9", "10", -1)]
        [InlineData("10", "9", 1)]
        [InlineData("1", "1", 0)]
        [InlineData("abc", "abc", 0)]
        [InlineData("file1", "file2", -1)]
        [InlineData("file2", "file10", -1)]
        [InlineData("file10", "file2", 1)]
        public void Compare_NumericSegments_SortsNumerically(string x, string y, int expectedSign)
        {
            var result = _sut.Compare(x, y);
            Assert.Equal(expectedSign, Math.Sign(result));
        }

        [Theory]
        [InlineData("2025W25", "2025W26", -1)]
        [InlineData("2025W26", "2025W25", 1)]
        [InlineData("2025W9", "2025W10", -1)]
        [InlineData("2025W25-26", "2025W25-27", -1)]
        public void Compare_SprintFormats_SortsCorrectly(string x, string y, int expectedSign)
        {
            var result = _sut.Compare(x, y);
            Assert.Equal(expectedSign, Math.Sign(result));
        }

        [Theory]
        [InlineData(null, null, 0)]
        [InlineData(null, "a", -1)]
        [InlineData("a", null, 1)]
        public void Compare_NullValues_HandlesCorrectly(string? x, string? y, int expectedSign)
        {
            var result = _sut.Compare(x, y);
            Assert.Equal(expectedSign, Math.Sign(result));
        }

        [Fact]
        public void Compare_MigrationPaths_SortsNaturally()
        {
            var paths = new List<string> { "10/migration.json", "2/migration.json", "1/migration.json", "9/migration.json" };

            paths.Sort(_sut);

            Assert.Equal(["1/migration.json", "2/migration.json", "9/migration.json", "10/migration.json"], paths);
        }

        [Fact]
        public void Compare_MixedSprintFormats_SortsNaturally()
        {
            var paths = new List<string> { "2025W10", "2025W2", "2025W1", "2025W9" };

            paths.Sort(_sut);

            Assert.Equal(["2025W1", "2025W2", "2025W9", "2025W10"], paths);
        }

        [Fact]
        public void Compare_CaseInsensitive()
        {
            var result = _sut.Compare("Sprint1", "sprint1");
            Assert.Equal(0, result);
        }
    }
}
