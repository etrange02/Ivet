namespace Ivet.Model
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyKeyAttribute : Attribute
    {
        public string? Name { get; set; }

        /// <summary>
        /// Quantity of data, SINGLE, LIST or SET.
        /// A LIST allows unique values instead of SET which allows duplicates
        /// </summary>
        public Cardinality Cardinality { get; set; } = Cardinality.SINGLE;
    }
}
