using Ivet.Model;
using Ivet.Model.Meta;
using Ivet.Services.Comparers;

namespace Ivet.Services
{
    public class DeltaSchemaMakerService
    {
        /// <summary>
        /// Normalize JanusGraph DataType strings for comparison.
        /// JanusGraph returns "class java.lang.String", code generates "String.class".
        /// </summary>
        private static string NormalizeDataType(string? dataType)
        {
            if (string.IsNullOrEmpty(dataType)) return string.Empty;

            // "class java.lang.String" → "String"
            if (dataType.StartsWith("class "))
            {
                var className = dataType["class ".Length..];
                var lastDot = className.LastIndexOf('.');
                if (lastDot >= 0) className = className[(lastDot + 1)..];
                return className;
            }

            // "String.class" → "String"
            if (dataType.EndsWith(".class"))
                return dataType[..^".class".Length];

            return dataType;
        }

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

        /// <summary>
        /// Detect elements present in source (database) but absent from target (code).
        /// These are elements that have been removed from the codebase.
        /// </summary>
        public MetaSchema Removals(MetaSchema source, MetaSchema target)
        {
            var result = new MetaSchema();
            result.Vertices.AddRange(source.Vertices.ExceptBy(target.Vertices.Select(x => x.Name), x => x.Name));
            result.Edges.AddRange(source.Edges.ExceptBy(target.Edges.Select(x => x.Name), x => x.Name));
            result.Properties.AddRange(source.Properties.ExceptBy(target.Properties.Select(x => x.Name), x => x.Name));
            result.Connections.AddRange(source.Connections.Except(target.Connections, new ConnectionComparer()));
            result.VertexPropertyBindings.AddRange(source.VertexPropertyBindings.Except(target.VertexPropertyBindings, new PropertyBindingComparer()));
            result.EdgePropertyBindings.AddRange(source.EdgePropertyBindings.Except(target.EdgePropertyBindings, new PropertyBindingComparer()));
            result.CompositeIndexes.AddRange(source.CompositeIndexes.Except(target.CompositeIndexes, new CompositeIndexComparer()));
            result.MixedIndexes.AddRange(source.MixedIndexes.Except(target.MixedIndexes, new MixedIndexComparer()));
            result.IndexBindings.AddRange(source.IndexBindings.Except(target.IndexBindings, new IndexBindingComparer()));

            return result;
        }

        /// <summary>
        /// Detect elements present in both source and target but with different properties.
        /// JanusGraph does not support modifying existing schema elements.
        /// </summary>
        public List<MetaSchemaModification> Modifications(MetaSchema source, MetaSchema target)
        {
            var modifications = new List<MetaSchemaModification>();

            foreach (var sourceVertex in source.Vertices)
            {
                var targetVertex = target.Vertices.FirstOrDefault(v => v.Name == sourceVertex.Name);
                if (targetVertex == null) continue;
                if (sourceVertex.Partitioned != targetVertex.Partitioned)
                    modifications.Add(new MetaSchemaModification { ElementType = "Vertex", ElementName = sourceVertex.Name, PropertyName = "Partitioned", SourceValue = sourceVertex.Partitioned.ToString(), TargetValue = targetVertex.Partitioned.ToString() });
                if (sourceVertex.Static != targetVertex.Static)
                    modifications.Add(new MetaSchemaModification { ElementType = "Vertex", ElementName = sourceVertex.Name, PropertyName = "Static", SourceValue = sourceVertex.Static.ToString(), TargetValue = targetVertex.Static.ToString() });
            }

            foreach (var sourceEdge in source.Edges)
            {
                var targetEdge = target.Edges.FirstOrDefault(e => e.Name == sourceEdge.Name);
                if (targetEdge == null) continue;
                if (sourceEdge.Multiplicity != targetEdge.Multiplicity)
                    modifications.Add(new MetaSchemaModification { ElementType = "Edge", ElementName = sourceEdge.Name, PropertyName = "Multiplicity", SourceValue = sourceEdge.Multiplicity.ToString(), TargetValue = targetEdge.Multiplicity.ToString() });
            }

            foreach (var sourceProperty in source.Properties)
            {
                var targetProperty = target.Properties.FirstOrDefault(p => p.Name == sourceProperty.Name);
                if (targetProperty == null) continue;
                if (sourceProperty.Cardinality != targetProperty.Cardinality)
                    modifications.Add(new MetaSchemaModification { ElementType = "PropertyKey", ElementName = sourceProperty.Name, PropertyName = "Cardinality", SourceValue = sourceProperty.Cardinality.ToString(), TargetValue = targetProperty.Cardinality.ToString() });
                if (NormalizeDataType(sourceProperty.DataType) != NormalizeDataType(targetProperty.DataType))
                    modifications.Add(new MetaSchemaModification { ElementType = "PropertyKey", ElementName = sourceProperty.Name, PropertyName = "DataType", SourceValue = sourceProperty.DataType ?? "", TargetValue = targetProperty.DataType ?? "" });
            }

            foreach (var sourceIndex in source.CompositeIndexes)
            {
                var targetIndex = target.CompositeIndexes.FirstOrDefault(i => i.Name == sourceIndex.Name);
                if (targetIndex == null) continue;
                if (sourceIndex.IsUnique != targetIndex.IsUnique)
                    modifications.Add(new MetaSchemaModification { ElementType = "CompositeIndex", ElementName = sourceIndex.Name, PropertyName = "IsUnique", SourceValue = sourceIndex.IsUnique.ToString(), TargetValue = targetIndex.IsUnique.ToString() });
            }

            foreach (var sourceMixedIndex in source.MixedIndexes)
            {
                var targetMixedIndex = target.MixedIndexes.FirstOrDefault(i => i.Name == sourceMixedIndex.Name);
                if (targetMixedIndex == null) continue;
                if (sourceMixedIndex.BackendIndex != targetMixedIndex.BackendIndex)
                    modifications.Add(new MetaSchemaModification { ElementType = "MixedIndex", ElementName = sourceMixedIndex.Name, PropertyName = "BackendIndex", SourceValue = sourceMixedIndex.BackendIndex, TargetValue = targetMixedIndex.BackendIndex });
            }

            var indexBindingComparer = new IndexBindingComparer();
            foreach (var sourceBinding in source.IndexBindings)
            {
                var targetBinding = target.IndexBindings.FirstOrDefault(b => indexBindingComparer.Equals(sourceBinding, b));
                if (targetBinding == null) continue;
                if (sourceBinding.Mapping != targetBinding.Mapping)
                    modifications.Add(new MetaSchemaModification { ElementType = "IndexBinding", ElementName = $"{sourceBinding.IndexName}.{sourceBinding.PropertyName}", PropertyName = "Mapping", SourceValue = sourceBinding.Mapping.ToString(), TargetValue = targetBinding.Mapping.ToString() });
            }

            return modifications;
        }
    }
}
