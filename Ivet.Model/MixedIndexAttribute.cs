namespace Ivet.Model
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MixedIndexAttribute : Attribute
    {
        /// <summary>
        /// Add the tagged property to an indexed managed by a search backend  (ie Sol'R or ElasticSearch)
        /// Impossible to modify it after creation.
        /// https://docs.janusgraph.org/index-management/index-performance/
        /// </summary>
        public string IndexName { get; private set; }

        /// <summary>
        /// Name of the backend system to search in. By default it is 'search'.
        /// </summary>
        public string Backend { get; set; } = "search";

        public MappingType Mapping { get; set; } = MappingType.NULL;

        public MixedIndexAttribute(string indexName)
        {
            IndexName = indexName;
        }
    }
}
