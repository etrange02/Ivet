using Ivet.Model;

namespace Ivet.Extensions
{
    public static class MappingTypeExtension
    {
        public static string ToJavaString(this MappingType type)
        {
            switch (type)
            {
                case MappingType.TEXT:
                    return "TEXT";
                case MappingType.TEXTSTRING:
                    return "TEXTSTRING";
                case MappingType.DEFAULT:
                    return "DEFAULT";
                case MappingType.STRING:
                    return "STRING";
                case MappingType.PREFIX_TREE:
                    return "PREFIX_TREE";
                default:
                    throw new ArgumentException($"Unknown mapping: {type}");
            }
        }
    }
}
