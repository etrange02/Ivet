namespace Ivet.TestFramework
{
    public class RandomGenerator
    {
        private static readonly Random _random = new();

        public static string RandomString() => Guid.NewGuid().ToString();
        public static int RandomInt() => _random.Next();
        public static int RandomInt(int max) => _random.Next() % (max + 1);
        public static int RandomInt(int min, int max) => (_random.Next() + min) % (max + 1);
        public static double RandomDouble() => _random.NextDouble();
        public static bool RandomBool() => RandomInt() % 2 == 0;
        public static Guid RandomGuid() => Guid.NewGuid();
        public static T RandomEnum<T>() where T : Enum
        {
            var enumValues = Enum.GetValues(typeof(T));
            return (T)enumValues.GetValue(_random.Next(enumValues.Length));
        }
    }
}