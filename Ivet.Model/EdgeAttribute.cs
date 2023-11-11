namespace Ivet.Model
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EdgeAttribute : AbstractGraphItemAttribute
    {
        public Multiplicity Multiplicity { get; set; } = Multiplicity.SIMPLE;
        public Type In { get; private set; }
        public Type Out { get; private set; }

        public EdgeAttribute(Type input, Type output)
        {
            In = input;
            Out = output;
        }
    }
}
