using Ivet.Model.Meta;
using Ivet.Services.Comparers;

namespace Ivet.Services
{
    public class DeltaSchemaMakerService
    {
        /// <summary>
        /// Generate difference between two schemas
        /// </summary>
        /// <param name="source">Schema currently in production</param>
        /// <param name="target">Schema we want to get</param>
        /// <returns></returns>
        public MetaSchema Difference(MetaSchema source, MetaSchema target)
        {
            var result = new MetaSchema();
            result.Vertices.AddRange(target.Vertices.ExceptBy(source.Vertices.Select(x => x.Name), x => x.Name));
            result.Edges.AddRange(target.Edges.ExceptBy(source.Edges.Select(x => x.Name), x => x.Name));
            result.Properties.AddRange(target.Properties.ExceptBy(source.Properties.Select(x => x.Name), x => x.Name));
            result.Connections.AddRange(target.Connections.Except(source.Connections, new ConnectionComparer()));
            result.VertexPropertyBindings.AddRange(target.VertexPropertyBindings.Except(source.VertexPropertyBindings, new PropertyBindingComparer()));
            result.EdgePropertyBindings.AddRange(target.EdgePropertyBindings.Except(source.EdgePropertyBindings, new PropertyBindingComparer()));
            result.CompositeIndexes.AddRange(target.CompositeIndexes.Except(source.CompositeIndexes, new CompositeIndexComparer()));
            result.MixedIndexes.AddRange(target.MixedIndexes.Except(source.MixedIndexes, new MixedIndexComparer()));
            result.IndexBindings.AddRange(target.IndexBindings.Except(source.IndexBindings, new IndexBindingComparer()));

            return result;
        }
    }
}
