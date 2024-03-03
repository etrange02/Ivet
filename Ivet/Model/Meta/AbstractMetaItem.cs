namespace Ivet.Model.Meta
{
    public abstract class AbstractMetaItem
    {
        public string Name { get; set; } = string.Empty;
        public Type? Type { get; set; }
    }

    public abstract class AbstractMetaItem<TAttribute> : AbstractMetaItem where TAttribute : AbstractGraphItemAttribute
    {
        public TAttribute? Attribute { get; set; }
    }
}
