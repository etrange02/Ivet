using Ivet.Model;
using Ivet.Model.Database;
using Ivet.Model.Meta;
using static Ivet.Services.GremlinIdentifierValidator;

namespace Ivet.Services.Converters
{
    public class DatabaseToSchemaConverter
    {
        public static MetaSchema Convert(Schema schema)
        {
            var result = new MetaSchema();

            result.Vertices.AddRange(schema.Vertices.ConvertAll(x => new MetaVertex
            {
                Name = Validate(x.Name, "vertex name"),
                Partitioned = x.Partitioned,
                Static = x.Static,
            }
            ));
            result.Properties.AddRange(schema.PropertyKeys.ConvertAll(x => new MetaPropertyKey
            {
                Name = Validate(x.Name, "property key name"),
                Cardinality = x.Cardinality,
                DataType = x.DataType,
            }
            ));
            result.Edges.AddRange(schema.Edges.ConvertAll(x => new MetaEdge
            {
                Name = Validate(x.Name, "edge name"),
                Multiplicity = x.Multiplicity,
            }));
            result.Connections.AddRange(schema.Connections.ConvertAll(x => new MetaConnection
            {
                Edge = Validate(x.Edge, "connection edge"),
                Ingoing = Validate(x.Ingoing, "connection ingoing"),
                Outgoing = Validate(x.Outgoing, "connection outgoing")
            }));
            result.VertexPropertyBindings.AddRange(schema.VertexPropertyBindings.ConvertAll(x => new MetaPropertyBinding
            {
                Name = Validate(x.Name, "vertex property binding name"),
                Entity = Validate(x.Entity, "vertex property binding entity")
            }));
            result.EdgePropertyBindings.AddRange(schema.EdgesPropertyBindings.ConvertAll(x => new MetaPropertyBinding
            {
                Name = Validate(x.Name, "edge property binding name"),
                Entity = Validate(x.Entity, "edge property binding entity")
            }));
            result.CompositeIndexes.AddRange(schema.Indices.Where(x => x.IsCompositeIndex).Select(x => new MetaCompositeIndex
            {
                Name = Validate(x.Name, "composite index name"),
                IsUnique = x.IsUnique,
                IndexedElement = Validate(x.IndexedElement, "composite index element"),
            }));
            result.MixedIndexes.AddRange(schema.Indices.Where(x => !x.IsCompositeIndex).Select(x => new MetaMixedIndex
            {
                Name = Validate(x.Name, "mixed index name"),
                BackendIndex = Validate(x.BackendIndex, "mixed index backend"),
                IndexedElement = Validate(x.IndexedElement, "mixed index element"),
            }));
            result.IndexBindings.AddRange(schema.IndexBindings
                .GroupBy(x => new { x.IndexName, x.PropertyName })
                .Select(g => new MetaIndexBinding
                {
                    IndexName = Validate(g.Key.IndexName, "index binding name"),
                    PropertyName = Validate(g.Key.PropertyName, "index binding property"),
                    Mapping = g.Select(x => Enum.TryParse<MappingType>(x.Parameter, out var m) && m != MappingType.NULL ? m : (MappingType?)null)
                        .FirstOrDefault(m => m != null) ?? MappingType.NULL,
                }));

            return result;
        }
    }
}
