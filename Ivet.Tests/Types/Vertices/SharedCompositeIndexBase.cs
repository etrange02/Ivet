using Ivet.Model;

namespace Ivet.Tests.Types.Vertices
{
    /// <summary>
    /// Test fixture for the auto fan-out behaviour of <see cref="CompositeIndexAttribute"/>.
    /// When the attribute is declared on an abstract base that has multiple concrete
    /// descendants, Ivet must produce one label-scoped composite per descendant, each name
    /// auto-prefixed with the concrete class name. JG composite indexes only support a
    /// single <c>indexOnly(label)</c>, so a shared attribute must fan out at migration time.
    /// </summary>
    public abstract class SharedCompositeIndexBase
    {
        [PropertyKey]
        [CompositeIndex("visibleToOrganizations")]
        public string SharedIndexedProperty { get; set; } = string.Empty;
    }

    [Vertex]
    public class SharedCompositeIndexVertexA : SharedCompositeIndexBase
    {
    }

    [Vertex]
    public class SharedCompositeIndexVertexB : SharedCompositeIndexBase
    {
    }
}
