namespace Ivet.TestFramework
{
    public class RandomGenerator
    {
        private static Random _random = new Random();

        public string RandomString() => Guid.NewGuid().ToString();
        public int RandomInt() => _random.Next();
        public int RandomInt(int max) => _random.Next() % (max + 1);
        public int RandomInt(int min, int max) => (_random.Next() + min) % (max + 1);
        public double RandomDouble() => _random.NextDouble();
        public bool RandomBool() => RandomInt() % 2 == 0;
        public Guid RandomGuid() => Guid.NewGuid();
        public T RandomEnum<T>() where T : Enum
        {
            var enumValues = Enum.GetValues(typeof(T));
            return (T)enumValues.GetValue(_random.Next(enumValues.Length));
        }
    }
}