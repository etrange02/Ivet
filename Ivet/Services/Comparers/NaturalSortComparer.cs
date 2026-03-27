using System.Text.RegularExpressions;

namespace Ivet.Services.Comparers
{
    /// <summary>
    /// Compares strings using natural sort order: numeric segments are compared
    /// as numbers so that "9" comes before "10", "2025W25" before "2025W26", etc.
    /// </summary>
    public partial class NaturalSortComparer : IComparer<string?>
    {
        public static readonly NaturalSortComparer Instance = new();

        [GeneratedRegex(@"(\d+)", RegexOptions.Compiled)]
        private static partial Regex NumericSegmentRegex();

        public int Compare(string? x, string? y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            var xSegments = NumericSegmentRegex().Split(x);
            var ySegments = NumericSegmentRegex().Split(y);

            var count = Math.Min(xSegments.Length, ySegments.Length);
            for (var i = 0; i < count; i++)
            {
                int result;
                if (int.TryParse(xSegments[i], out var xNum) && int.TryParse(ySegments[i], out var yNum))
                    result = xNum.CompareTo(yNum);
                else
                    result = string.Compare(xSegments[i], ySegments[i], StringComparison.OrdinalIgnoreCase);

                if (result != 0) return result;
            }

            return xSegments.Length.CompareTo(ySegments.Length);
        }
    }
}
