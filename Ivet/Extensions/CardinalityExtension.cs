using Ivet.Model;

namespace Ivet.Extensions
{
    public static class CardinalityExtension
    {
        public static string ToJavaString(this Cardinality cardinality)
        {
            switch (cardinality)
            {
                case Cardinality.SINGLE:
                    return "org.janusgraph.core.Cardinality.SINGLE";
                case Cardinality.SET:
                    return "org.janusgraph.core.Cardinality.SET";
                case Cardinality.LIST:
                    return "org.janusgraph.core.Cardinality.LIST";
                default:
                    throw new ArgumentException($"Unknown cardinality: {cardinality}");
            }
        }
    }
}