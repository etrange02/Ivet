using Ivet.Model;

namespace Ivet.Extensions
{
    public static class MultiplicityExtension
    {
        public static string ToJavaString(this Multiplicity multiplicty)
        {
            switch (multiplicty)
            {
                case Multiplicity.MULTI:
                    return "MULTI";
                case Multiplicity.SIMPLE:
                    return "SIMPLE";
                case Multiplicity.MANY2ONE:
                    return "MANY2ONE";
                case Multiplicity.ONE2MANY:
                    return "ONE2MANY";
                case Multiplicity.ONE2ONE:
                    return "ONE2ONE";
                default:
                    throw new ArgumentException($"Unknown multiplicty: {multiplicty}");
            }
        }
    }
}