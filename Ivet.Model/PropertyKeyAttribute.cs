namespace Ivet.Model
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyKeyAttribute : Attribute
    {
        public string? Name { get; set; }

        /// <summary>
        /// Quantity of data, SINGLE, LIST or SET.
        /// A SET allows unique values while LIST allows duplicates.
        /// </summary>
        public Cardinality Cardinality { get; set; } = Cardinality.SINGLE;
        public bool EnumAsString { get; set; }
    }
}
