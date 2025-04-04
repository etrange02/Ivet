using System.Reflection;

namespace Ivet.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static string ToJavaType(this PropertyInfo memberInfo)
        {
            if (memberInfo.PropertyType == typeof(short) || memberInfo.PropertyType == typeof(short?) || memberInfo.PropertyType == typeof(short[]) || typeof(IEnumerable<short>).IsAssignableFrom(memberInfo.PropertyType) || typeof(IEnumerable<short?>).IsAssignableFrom(memberInfo.PropertyType))
                return "Short.class";
            if (memberInfo.PropertyType == typeof(int) || memberInfo.PropertyType == typeof(int?) || memberInfo.PropertyType == typeof(int[]) || typeof(IEnumerable<int>).IsAssignableFrom(memberInfo.PropertyType) || typeof(IEnumerable<int?>).IsAssignableFrom(memberInfo.PropertyType))
                return "Integer.class";
            if (memberInfo.PropertyType == typeof(long) || memberInfo.PropertyType == typeof(long?) || memberInfo.PropertyType == typeof(long[]) || typeof(IEnumerable<long>).IsAssignableFrom(memberInfo.PropertyType) || typeof(IEnumerable<long?>).IsAssignableFrom(memberInfo.PropertyType))
                return "Long.class";
            if (memberInfo.PropertyType == typeof(char) || memberInfo.PropertyType == typeof(char?) || memberInfo.PropertyType == typeof(char[]) || typeof(IEnumerable<char>).IsAssignableFrom(memberInfo.PropertyType) || typeof(IEnumerable<char?>).IsAssignableFrom(memberInfo.PropertyType))
                return "Char.class";
            if (memberInfo.PropertyType == typeof(string) || /*memberInfo.PropertyType is string? ||*/ memberInfo.PropertyType == typeof(string[]) || typeof(IEnumerable<string>).IsAssignableFrom(memberInfo.PropertyType))
                return "String.class";
            if (memberInfo.PropertyType == typeof(bool) || memberInfo.PropertyType == typeof(bool?) || memberInfo.PropertyType == typeof(bool[]) || typeof(IEnumerable<bool>).IsAssignableFrom(memberInfo.PropertyType) || typeof(IEnumerable<bool?>).IsAssignableFrom(memberInfo.PropertyType))
                return "Boolean.class";
            if (memberInfo.PropertyType == typeof(float) || memberInfo.PropertyType == typeof(float?) || memberInfo.PropertyType == typeof(float[]) || typeof(IEnumerable<float>).IsAssignableFrom(memberInfo.PropertyType) || typeof(IEnumerable<float?>).IsAssignableFrom(memberInfo.PropertyType))
                return "Float.class";
            if (memberInfo.PropertyType == typeof(double) || memberInfo.PropertyType == typeof(double?) || memberInfo.PropertyType == typeof(double[]) || typeof(IEnumerable<double>).IsAssignableFrom(memberInfo.PropertyType) || typeof(IEnumerable<double?>).IsAssignableFrom(memberInfo.PropertyType))
                return "Double.class";
            if (memberInfo.PropertyType == typeof(byte) || memberInfo.PropertyType == typeof(byte?) || memberInfo.PropertyType == typeof(byte[]) || typeof(IEnumerable<byte>).IsAssignableFrom(memberInfo.PropertyType) || typeof(IEnumerable<byte?>).IsAssignableFrom(memberInfo.PropertyType))
                return "Byte.class";
            if (memberInfo.PropertyType == typeof(DateTime) || memberInfo.PropertyType == typeof(DateTime?) || memberInfo.PropertyType == typeof(DateTime[]) || typeof(IEnumerable<DateTime>).IsAssignableFrom(memberInfo.PropertyType) || typeof(IEnumerable<DateTime?>).IsAssignableFrom(memberInfo.PropertyType)
                || memberInfo.PropertyType == typeof(DateOnly) || memberInfo.PropertyType == typeof(DateOnly?) || memberInfo.PropertyType == typeof(DateOnly[]) || typeof(IEnumerable<DateOnly>).IsAssignableFrom(memberInfo.PropertyType) || typeof(IEnumerable<DateOnly?>).IsAssignableFrom(memberInfo.PropertyType)
                || memberInfo.PropertyType == typeof(TimeOnly) || memberInfo.PropertyType == typeof(TimeOnly?) || memberInfo.PropertyType == typeof(TimeOnly[]) || typeof(IEnumerable<TimeOnly>).IsAssignableFrom(memberInfo.PropertyType) || typeof(IEnumerable<TimeOnly?>).IsAssignableFrom(memberInfo.PropertyType))
                return "Date.class";
            if (memberInfo.PropertyType == typeof(Guid) || memberInfo.PropertyType == typeof(Guid?) || memberInfo.PropertyType == typeof(Guid[]) || typeof(IEnumerable<Guid>).IsAssignableFrom(memberInfo.PropertyType) || typeof(IEnumerable<Guid?>).IsAssignableFrom(memberInfo.PropertyType))
                return "UUID.class";
            throw new ArgumentException($"Type unknown: {memberInfo.PropertyType.Name}");
        }
    }
}