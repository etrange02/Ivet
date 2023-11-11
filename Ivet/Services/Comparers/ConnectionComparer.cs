using Ivet.Model.Meta;
using System.Diagnostics.CodeAnalysis;

namespace Ivet.Services.Comparers
{
    public class ConnectionComparer : IEqualityComparer<MetaConnection>
    {
        public bool Equals(MetaConnection? x, MetaConnection? y)
        {
            if (x == null)
                return y == null;
            else if (y == null)
                return false;
            else
                return x.Edge == y.Edge && x.Ingoing == y.Ingoing && x.Outgoing == y.Outgoing;
        }

        public int GetHashCode([DisallowNull] MetaConnection obj)
        {
            return obj.Edge.GetHashCode();
        }
    }
}
