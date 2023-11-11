namespace Ivet.Model
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CompositeIndexAttribute : Attribute
    {
        public string IndexName { get; private set; }
        public bool IsUnique { get; set; }

        public CompositeIndexAttribute(string indexName)
        {
            IndexName = indexName;
        }
    }
}
