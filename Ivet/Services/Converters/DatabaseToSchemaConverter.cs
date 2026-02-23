using Ivet.Model;
using Ivet.Model.Database;
using Ivet.Model.Meta;

namespace Ivet.Services.Converters
{
    public class DatabaseToSchemaConverter
    {
        public static MetaSchema Convert(Schema schema)
        {
            var result = new MetaSchema();

            result.Vertices.AddRange(schema.Vertices.ConvertAll(x => new MetaVertex
            {
                Name = x.Name,
                Partitioned = x.Partitioned,
                Static = x.Static,
            }
            ));
            result.Properties.AddRange(schema.PropertyKeys.ConvertAll(x => new MetaPropertyKey
            {
                Name = x.Name,
                Cardinality = x.Cardinality,
                DataType = x.DataType,
            }
            ));
            result.Edges.AddRange(schema.Edges.ConvertAll(x => new MetaEdge
            {
                Name = x.Name,
                Multiplicity = x.Multiplicity,
            }));
            result.Connections.AddRange(schema.Connections.ConvertAll(x => new MetaConnection
            {
                Edge = x.Edge,
                Ingoing = x.Ingoing,
                Outgoing = x.Outgoing
            }));
            result.VertexPropertyBindings.AddRange(schema.VertexPropertyBindings.ConvertAll(x => new MetaPropertyBinding
            {
                Name = x.Name,
                Entity = x.Entity
            }));
            result.EdgePropertyBindings.AddRange(schema.EdgesPropertyBindings.ConvertAll(x => new MetaPropertyBinding
            {
                Name = x.Name,
                Entity = x.Entity
            }));
            result.CompositeIndexes.AddRange(schema.Indices.Where(x => x.IsCompositeIndex).Select(x => new MetaCompositeIndex
            {
                Name = x.Name,
                IsUnique = x.IsUnique,
                IndexedElement = x.IndexedElement,
            }));
            result.MixedIndexes.AddRange(schema.Indices.Where(x => !x.IsCompositeIndex).Select(x => new MetaMixedIndex
            {
                Name = x.Name,
                BackendIndex = x.BackendIndex,
                IndexedElement = x.IndexedElement,
            }));
            result.IndexBindings.AddRange(schema.IndexBindings
                .GroupBy(x => new { x.IndexName, x.PropertyName })
                .Select(g => new MetaIndexBinding
                {
                    IndexName = g.Key.IndexName,
                    PropertyName = g.Key.PropertyName,
                    Mapping = g.Select(x => Enum.TryParse<MappingType>(x.Parameter, out var m) && m != MappingType.NULL ? m : (MappingType?)null)
                        .FirstOrDefault(m => m != null) ?? MappingType.NULL,
                }));

            return result;
        }
    }
}
