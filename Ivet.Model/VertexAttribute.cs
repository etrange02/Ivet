namespace Ivet.Model
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class VertexAttribute : AbstractGraphItemAttribute
    {
        /**
         * Indicates a vertex is cut. A copy is created in each node of the cluster. Avoid a hot spot
         */
        public bool Partitioned { get; set; }
        /**
         * Indicates instances of this kind cannot be removed or modified. It is fixed and can not be updgraded/downgraded.
         */
        public bool Static { get; set; }
    }
}
