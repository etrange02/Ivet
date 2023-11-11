namespace Ivet.Model
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class EdgePropertyAttribute : Attribute
    {
        public string? Name { get; set; }
        public Multiplicity Multiplicity { get; set; } = Multiplicity.ONE2MANY;
        public Type? Out { get; private set; }
    }
}
