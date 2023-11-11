namespace Ivet.Model
{
    /// <summary>
    /// Properties with this attribute are set to be the primary key, i.e. uniqueness is checked for create and update
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PrimaryKeyAttribute : Attribute
    {
    }
}
