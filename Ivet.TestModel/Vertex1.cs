using Ivet.Model;

namespace Ivet.TestModel
{
    [Vertex]
    public class Vertex1
    {
        [PropertyKey]
        [PrimaryKey()]
        public string Id { get; set; }

        [EdgeProperty]
        public List<Vertex3> Vertex3s { get; private set; } = new List<Vertex3>();

        [EdgeProperty]
        public Vertex3[] ArrayVertex3s { get; private set; } = Array.Empty<Vertex3>();

        [PropertyKey]
        public short Short { get; set; }
        [PropertyKey]
        public short? NullableShort { get; set; }
        [PropertyKey]
        public short[] ShortArray { get; set; }
        [PropertyKey]
        public IEnumerable<short> ShortEnumerable { get; set; }

        [PropertyKey]
        public int Integer { get; set; }
        [PropertyKey]
        public int? NullableInteger { get; set; }
        [PropertyKey]
        public int[] IntegerArray { get; set; }
        [PropertyKey]
        public int[]? IntegerArrayNullable { get; set; }
        [PropertyKey]
        public IEnumerable<int> IntegerEnumerable { get; set; }
        [PropertyKey]
        public IEnumerable<int?> NullableIntegerEnumerable { get; set; }
        [PropertyKey]
        public List<int> IntegerList { get; set; }

        [PropertyKey]
        public long Long { get; set; }
        [PropertyKey]
        public long? NullableLong { get; set; }
        [PropertyKey]
        public long[] LongArray { get; set; }
        [PropertyKey]
        public IEnumerable<long> LongEnumerable { get; set; }


        [PropertyKey]
        public char Charracter { get; set; }
        [PropertyKey]
        public char? NullableCharracter { get; set; }
        [PropertyKey]
        public char[] CharracterArray { get; set; }
        [PropertyKey]
        public IEnumerable<char> CharracterEnumerable { get; set; }

        [PropertyKey]
        public string String { get; set; }
        [PropertyKey]
        public string[] StringArray { get; set; }
        [PropertyKey]
        public IEnumerable<string> StringEnumerable { get; set; }

        [PropertyKey]
        public bool Boolean { get; set; }
        [PropertyKey]
        public bool? NullableBoolean { get; set; }
        [PropertyKey]
        public bool[] BooleanArray { get; set; }
        [PropertyKey]
        public IEnumerable<bool> BooleanEnumerable { get; set; }

        [PropertyKey]
        public float Float { get; set; }
        [PropertyKey]
        public float? NullableFloat { get; set; }
        [PropertyKey]
        public float[] FloatArray { get; set; }
        [PropertyKey]
        public IEnumerable<float> FloatEnumerable { get; set; }

        [PropertyKey]
        public double Double { get; set; }
        [PropertyKey]
        public double? NullableDouble { get; set; }
        [PropertyKey]
        public double[] DoubleArray { get; set; }
        [PropertyKey]
        public IEnumerable<double> DoubleEnumerable { get; set; }

        [PropertyKey]
        public byte Byte { get; set; }
        [PropertyKey]
        public byte? ByteDouble { get; set; }
        [PropertyKey]
        public byte[] ByteArray { get; set; }
        [PropertyKey]
        public IEnumerable<byte> ByteEnumerable { get; set; }

        [PropertyKey]
        public DateTime DateTime { get; set; }
        [PropertyKey]
        public DateTime? NullableDateTime { get; set; }
        [PropertyKey]
        public DateTime[] DateTimeArray { get; set; }
        [PropertyKey]
        public IEnumerable<DateTime> DateTimeEnumerable { get; set; }

        [PropertyKey]
        public DateOnly DateOnly { get; set; }
        [PropertyKey]
        public DateOnly? NullableDateOnly { get; set; }
        [PropertyKey]
        public DateOnly[] DateOnlyArray { get; set; }
        [PropertyKey]
        public IEnumerable<DateOnly> DateOnlyEnumerable { get; set; }

        [PropertyKey]
        public TimeOnly TimeOnly { get; set; }
        [PropertyKey]
        public TimeOnly? NullableTimeOnly { get; set; }
        [PropertyKey]
        public TimeOnly[] TimeOnlyArray { get; set; }
        [PropertyKey]
        public IEnumerable<TimeOnly> TimeOnlyEnumerable { get; set; }

        [PropertyKey]
        public Guid Guid { get; set; }
        [PropertyKey]
        public Guid? NullableGuid { get; set; }
        [PropertyKey]
        public Guid[] GuidArray { get; set; }
        [PropertyKey]
        public IEnumerable<Guid> GuidEnumerable { get; set; }
    }
}