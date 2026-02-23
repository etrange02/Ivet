namespace Ivet.TestFramework
{
    public class RandomGenerator
    {
        public static string RandomString() => Guid.NewGuid().ToString();
        public static int RandomInt() => Random.Shared.Next();
        public static int RandomInt(int max) => Random.Shared.Next(max + 1);
        public static int RandomInt(int min, int max) => Random.Shared.Next(min, max + 1);
        public static double RandomDouble() => Random.Shared.NextDouble();
        public static bool RandomBool() => RandomInt() % 2 == 0;
        public static Guid RandomGuid() => Guid.NewGuid();
        public static T RandomEnum<T>() where T : Enum
        {
            var enumValues = Enum.GetValues(typeof(T));
            return (T)enumValues.GetValue(Random.Shared.Next(enumValues.Length))!;
        }
    }
}