using Ivet.Model.Meta;
using System.Diagnostics.CodeAnalysis;

namespace Ivet.Services.Comparers
{
    public class IndexBindingComparer : IEqualityComparer<MetaIndexBinding>
    {
        public bool Equals(MetaIndexBinding? x, MetaIndexBinding? y)
        {
            if (x == null)
                return y == null;
            else if (y == null)
                return false;
            else
                return x.IndexName == y.IndexName && x.PropertyName == y.PropertyName;
        }

        public int GetHashCode([DisallowNull] MetaIndexBinding obj)
        {
            return obj.IndexName.GetHashCode();
        }
    }
}
