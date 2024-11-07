using System.Reflection;

namespace Ivet.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static string ToJavaType(this PropertyInfo memberInfo)
        {
            if (memberInfo.PropertyType == typeof(short) || memberInfo.PropertyType == typeof(short?) || memberInfo.PropertyType == typeof(short[]) || memberInfo.PropertyType == typeof(IEnumerable<short>))
                return "Short.class";
            if (memberInfo.PropertyType == typeof(int) || memberInfo.PropertyType == typeof(int?) || memberInfo.PropertyType == typeof(int[]) || memberInfo.PropertyType == typeof(IEnumerable<int>))
                return "Integer.class";
            if (memberInfo.PropertyType == typeof(long) || memberInfo.PropertyType == typeof(long?) || memberInfo.PropertyType == typeof(long[]) || memberInfo.PropertyType == typeof(IEnumerable<long>))
                return "Long.class";
            if (memberInfo.PropertyType == typeof(char) || memberInfo.PropertyType == typeof(char?) || memberInfo.PropertyType == typeof(char[]) || memberInfo.PropertyType == typeof(IEnumerable<char>))
                return "Char.class";
            if (memberInfo.PropertyType == typeof(string) || /*memberInfo.PropertyType == typeof(string?) ||*/ memberInfo.PropertyType == typeof(string[]) || memberInfo.PropertyType == typeof(IEnumerable<string>))
                return "String.class";
            if (memberInfo.PropertyType == typeof(bool) || memberInfo.PropertyType == typeof(bool?) || memberInfo.PropertyType == typeof(bool[]) || memberInfo.PropertyType == typeof(IEnumerable<bool>))
                return "Boolean.class";
            if (memberInfo.PropertyType == typeof(float) || memberInfo.PropertyType == typeof(float?) || memberInfo.PropertyType == typeof(float[]) || memberInfo.PropertyType == typeof(IEnumerable<float>))
                return "Float.class";
            if (memberInfo.PropertyType == typeof(double) || memberInfo.PropertyType == typeof(double?) || memberInfo.PropertyType == typeof(double[]) || memberInfo.PropertyType == typeof(IEnumerable<double>))
                return "Double.class";
            if (memberInfo.PropertyType == typeof(byte) || memberInfo.PropertyType == typeof(byte?) || memberInfo.PropertyType == typeof(byte[]) || memberInfo.PropertyType == typeof(IEnumerable<byte>))
                return "Byte.class";
            if (memberInfo.PropertyType == typeof(DateTime) || memberInfo.PropertyType == typeof(DateTime?) || memberInfo.PropertyType == typeof(DateTime[]) || memberInfo.PropertyType == typeof(IEnumerable<DateTime>)
                || memberInfo.PropertyType == typeof(DateOnly) || memberInfo.PropertyType == typeof(DateOnly?) || memberInfo.PropertyType == typeof(DateOnly[]) || memberInfo.PropertyType == typeof(IEnumerable<DateOnly>)
                || memberInfo.PropertyType == typeof(TimeOnly) || memberInfo.PropertyType == typeof(TimeOnly?) || memberInfo.PropertyType == typeof(TimeOnly[]) || memberInfo.PropertyType == typeof(IEnumerable<TimeOnly>))
                return "Date.class";
            if (memberInfo.PropertyType == typeof(Guid) || memberInfo.PropertyType == typeof(Guid?) || memberInfo.PropertyType == typeof(Guid[]) || memberInfo.PropertyType == typeof(IEnumerable<Guid>))
                return "UUID.class";
            throw new ArgumentException($"Type unknown: {memberInfo.PropertyType.Name}");
        }
    }
}