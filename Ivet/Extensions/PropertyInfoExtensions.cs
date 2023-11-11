using System.Reflection;

namespace Ivet.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static string ToJavaType(this PropertyInfo memberInfo)
        {
            if (memberInfo.PropertyType == typeof(int) || memberInfo.PropertyType == typeof(int?) || memberInfo.PropertyType == typeof(int[]) || memberInfo.PropertyType == typeof(IEnumerable<int>))
                return "Integer.class";
            if (memberInfo.PropertyType == typeof(string) || /*memberInfo.PropertyType == typeof(string?) ||*/ memberInfo.PropertyType == typeof(string[]) || memberInfo.PropertyType == typeof(IEnumerable<string>))
                return "String.class";
            if (memberInfo.PropertyType == typeof(bool) || memberInfo.PropertyType == typeof(bool?) || memberInfo.PropertyType == typeof(bool[]) || memberInfo.PropertyType == typeof(IEnumerable<bool>))
                return "Boolean.class";
            if (memberInfo.PropertyType == typeof(double) || memberInfo.PropertyType == typeof(double?) || memberInfo.PropertyType == typeof(double[]) || memberInfo.PropertyType == typeof(IEnumerable<double>))
                return "Double.class";
            if (memberInfo.PropertyType == typeof(DateTime) || memberInfo.PropertyType == typeof(DateTime?) || memberInfo.PropertyType == typeof(DateTime[]) || memberInfo.PropertyType == typeof(IEnumerable<DateTime>))
                return "Date.class";
            throw new ArgumentException($"Type unknown: {memberInfo.PropertyType.Name}");
        }
    }
}