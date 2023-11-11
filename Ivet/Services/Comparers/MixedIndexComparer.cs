using Ivet.Model.Meta;
using System.Diagnostics.CodeAnalysis;

namespace Ivet.Services.Comparers
{
    public class MixedIndexComparer : IEqualityComparer<MetaMixedIndex>
    {
        public bool Equals(MetaMixedIndex? x, MetaMixedIndex? y)
        {
            if (x == null)
                return y == null;
            else if (y == null)
                return false;
            else
                return x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] MetaMixedIndex obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
