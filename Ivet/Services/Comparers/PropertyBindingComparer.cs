using Ivet.Model.Meta;
using System.Diagnostics.CodeAnalysis;

namespace Ivet.Services.Comparers
{
    public class PropertyBindingComparer : IEqualityComparer<MetaPropertyBinding>
    {
        public bool Equals(MetaPropertyBinding? x, MetaPropertyBinding? y)
        {
            if (x == null)
                return y == null;
            else if (y == null)
                return false;
            else
                return x.Name == y.Name && x.Entity == y.Entity;
        }

        public int GetHashCode([DisallowNull] MetaPropertyBinding obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
