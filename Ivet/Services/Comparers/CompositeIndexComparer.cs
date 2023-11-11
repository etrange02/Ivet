using Ivet.Model.Meta;
using System.Diagnostics.CodeAnalysis;

namespace Ivet.Services.Comparers
{
    public class CompositeIndexComparer : IEqualityComparer<MetaCompositeIndex>
    {
        public bool Equals(MetaCompositeIndex? x, MetaCompositeIndex? y)
        {
            if (x == null)
                return y == null;
            else if (y == null)
                return false;
            else
                return x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] MetaCompositeIndex obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
