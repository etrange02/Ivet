using Ivet.Extensions;
using Ivet.Model;
using Ivet.Model.Library;
using Ivet.Model.Meta;
using System.Reflection;
using static Ivet.Services.GremlinIdentifierValidator;

namespace Ivet.Services.Converters
{
    public class LibraryToSchemaConverter
    {
        public static MetaSchema Convert(Schema schema)
        {
            var result = new MetaSchema();

            result.Vertices.AddRange(GetVertices(schema));
            result.Edges.AddRange(GetEdges(schema));
            result.Properties.AddRange(GetProperties(schema));
            result.Connections.AddRange(GetConnections(result));
            result.VertexPropertyBindings.AddRange(GetVertexPropertyBindings(result));
            result.EdgePropertyBindings.AddRange(GetEdgePropertyBindings(result));
            result.CompositeIndexes.AddRange(GetCompositeIndices(result));
            result.MixedIndexes.AddRange(GetMixedIndices(schema));
            result.IndexBindings.AddRange(GetIndexBindings(result));

            return result;
        }

        private static IEnumerable<MetaVertex> GetVertices(Schema schema)
        {
            return schema.Vertices.ConvertAll(x =>
            {
                var attribute = x.GetCustomAttribute<VertexAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on { x.FullName }");
                return new MetaVertex
                {
                    Name = Validate(attribute.Name ?? x.Name, "vertex name"),
                    Partitioned = attribute.Partitioned,
                    Static = attribute.Static,
                    Type = x,
                    Attribute = attribute
                };
            }).DistinctBy(x => x.Name);
        }

        private static IEnumerable<MetaEdge> GetEdges(Schema schema)
        {
            return schema.Edges.SelectMany(x =>
            {
                var attributes = x.GetCustomAttributes<EdgeAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on {x.FullName}");
                return attributes.Select(attribute => new MetaEdge
                    {
                        Name = Validate(attribute.Name ?? x.Name, "edge name"),
                        Multiplicity = attribute.Multiplicity,
                        Type = x,
                        Attribute = attribute,
                        In = attribute.In,
                        Out = attribute.Out
                    });

            }).DistinctBy(x => $"{x.Name}-{x.In?.Name}-{x.Out?.Name}")
            .Concat(schema.Vertices.SelectMany(x =>
            {
                var properties = x.GetProperties().Where(y => y.GetCustomAttribute<EdgePropertyAttribute>() != null);

                return properties.Select(y =>
                {
                    var attribute = y.GetCustomAttribute<EdgePropertyAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on {x.FullName}");
                    var type = y.PropertyType.IsGenericType ? y.PropertyType.GenericTypeArguments[0] : y.PropertyType.GetElementType();
                    return new MetaEdge
                    {
                        Name = Validate(attribute.Name ?? $"{x.Name}_{y.Name}", "edge name"),
                        Multiplicity = attribute.Multiplicity,
                        Type = null,
                        Attribute = null,
                        In = x,
                        Out = attribute.Out ?? type
                    };
                });
            }));
        }

        private static IEnumerable<MetaPropertyKey> GetProperties(Schema schema)
        {
            return schema.Vertices.Concat(schema.Edges).SelectMany(x =>
            {
                var properties = x.GetProperties().Where(y => y.GetCustomAttribute<PropertyKeyAttribute>(true) != null);

                return properties.Select(y =>
                {
                    var attribute = y.GetCustomAttribute<PropertyKeyAttribute>(true) ?? throw new AttributeNotFoundException($"Attribute not found on {x.FullName}");
                    return new MetaPropertyKey
                    {
                        Name = Validate(attribute.Name ?? y.Name, "property key name"),
                        Cardinality = attribute.Cardinality,
                        DataType = y.ToJavaType(attribute),
                        PropertyInfo = y,
                    };
                });
            }).DistinctBy(x => x.Name);
        }

        private static IEnumerable<MetaConnection> GetConnections(MetaSchema metaSchema)
        {
            return metaSchema.Edges.Select(x =>
            {
                var inMetaVertex = metaSchema.Vertices.FirstOrDefault(y => y.Type == x.In);
                var outMetaVertex = metaSchema.Vertices.FirstOrDefault(y => y.Type == x.Out);

                return new MetaConnection
                {
                    Edge = x.Name,
                    Ingoing = inMetaVertex?.Name,
                    Outgoing = outMetaVertex?.Name
                };
            });
        }

        private static IEnumerable<MetaPropertyBinding> GetVertexPropertyBindings(MetaSchema metaSchema)
        {
            return GetPropertyBindings(metaSchema.Vertices);
        }

        private static IEnumerable<MetaPropertyBinding> GetEdgePropertyBindings(MetaSchema metaSchema)
        {
            return GetPropertyBindings(metaSchema.Edges);
        }

        private static IEnumerable<MetaPropertyBinding> GetPropertyBindings(IEnumerable<AbstractMetaItem> items)
        {
            return items.Where(x => x.Type != null).SelectMany(x =>
            {
                var properties = x.Type.GetProperties().Where(y => y.GetCustomAttribute<PropertyKeyAttribute>(true) != null);

                return properties.Select(y =>
                {
                    var attribute = y.GetCustomAttribute<PropertyKeyAttribute>(true);
                    return new MetaPropertyBinding
                    {
                        Name = attribute?.Name ?? y.Name,
                        Entity = x.Name
                    };
                });
            });
        }

        private static IEnumerable<MetaCompositeIndex> GetCompositeIndices(MetaSchema schema)
        {
            return GetCompositeIndexAttributes(schema.Vertices)
                .Concat(GetCompositeIndexAttributes(schema.Edges.Where(x => x.Type != null)))
                .Concat(GetPrimaryKeyIndices(schema.Vertices))
                .Concat(GetPrimaryKeyIndices(schema.Edges.Where(x => x.Type != null)))
                .DistinctBy(x => x.Name);
        }

        // [CompositeIndex] is AllowMultiple = true: a property may carry several attributes. Iterates per attribute.
        // A composite index can legitimately span multiple properties (including a [PrimaryKey] one) — do NOT dedup
        // against [PrimaryKey] here. The user is responsible for not declaring two single-property indexes with
        // different names on the same property.
        private static IEnumerable<MetaCompositeIndex> GetCompositeIndexAttributes(IEnumerable<AbstractMetaItem> items)
        {
            return items.SelectMany(x =>
            {
                var kind = x.Type.GetCustomAttribute<VertexAttribute>() != null ? "Vertex.class" : "Edge.class";
                var properties = x.Type.GetProperties().Where(y =>
                    y.GetCustomAttribute<PropertyKeyAttribute>(true) != null &&
                    y.GetCustomAttributes<CompositeIndexAttribute>().Any());

                return properties.SelectMany(y => y.GetCustomAttributes<CompositeIndexAttribute>().Select(attr => new MetaCompositeIndex
                {
                    Name = Validate(attr.IndexName, "composite index name"),
                    IsUnique = attr.IsUnique,
                    IndexedElement = x.Name,
                    Kind = kind
                }));
            });
        }

        private static IEnumerable<MetaCompositeIndex> GetPrimaryKeyIndices(IEnumerable<AbstractMetaItem> items)
        {
            return items.SelectMany(x =>
            {
                var properties = x.Type.GetProperties().Where(y =>
                    y.GetCustomAttribute<PropertyKeyAttribute>(true) != null &&
                    y.GetCustomAttribute<PrimaryKeyAttribute>() != null).ToList();
                if (properties.Count == 0) return Enumerable.Empty<MetaCompositeIndex>();

                // EdgeAttribute is AllowMultiple = true (an edge can have several In/Out pairs); use FirstOrDefault to avoid AmbiguousMatchException
                var graphItemAttribute = x.Type.GetCustomAttributes<AbstractGraphItemAttribute>().FirstOrDefault() ?? throw new AttributeNotFoundException($"Attribute not found on {x.Type.FullName}");
                var kind = x.Type.GetCustomAttribute<VertexAttribute>() != null ? "Vertex.class" : "Edge.class";

                return properties.Select(_ => new MetaCompositeIndex
                {
                    Name = Validate(graphItemAttribute.Name ?? $"{x.Name}_PK", "primary key index name"),
                    IsUnique = true,
                    IndexedElement = x.Name,
                    Kind = kind
                });
            });
        }

        private static IEnumerable<MetaMixedIndex> GetMixedIndices(Schema schema)
        {
            // Collect all (indexName, vertexName) pairs — a property may carry multiple [MixedIndex] attributes
            var allEntries = schema.Vertices.Concat(schema.Edges).SelectMany(x =>
            {
                var properties = x.GetProperties().Where(y =>
                    y.GetCustomAttribute<PropertyKeyAttribute>(true) != null &&
                    y.GetCustomAttributes<MixedIndexAttribute>().Any());

                if (!x.GetCustomAttributes<AbstractGraphItemAttribute>().Any()) return Enumerable.Empty<MetaMixedIndex>();

                return properties.SelectMany(y =>
                    y.GetCustomAttributes<MixedIndexAttribute>().Select(attr => new MetaMixedIndex
                    {
                        Name = Validate(attr.IndexName, "mixed index name"),
                        BackendIndex = Validate(attr.Backend, "mixed index backend"),
                        IndexedElement = x.Name,
                        Kind = x.GetCustomAttribute<VertexAttribute>() != null ? "Vertex.class" : "Edge.class"
                    })
                );
            }).ToList();

            // Group by index name. If multiple vertex types share the same index → global (no indexOnly)
            return allEntries
                .GroupBy(x => x.Name)
                .Select(g =>
                {
                    var first = g.First();
                    var distinctVertices = g.Select(x => x.IndexedElement).Distinct().Count();
                    return new MetaMixedIndex
                    {
                        Name = first.Name,
                        BackendIndex = first.BackendIndex,
                        IndexedElement = distinctVertices > 1 ? string.Empty : first.IndexedElement,
                        Kind = first.Kind
                    };
                });
        }

        private static IEnumerable<MetaIndexBinding> GetIndexBindings(MetaSchema schema)
        {
            return GetCompositeIndexBindings(schema.Vertices)
                .Concat(GetCompositeIndexBindings(schema.Edges.Where(x => x.Type != null)))
                .Concat(GetPrimaryKeyBindings(schema.Vertices))
                .Concat(GetPrimaryKeyBindings(schema.Edges.Where(x => x.Type != null)))
                .Concat(GetAllMixedIndexBindings(schema.Vertices))
                .Concat(GetAllMixedIndexBindings(schema.Edges.Where(x => x.Type != null)))
                .DistinctBy(x => $"{x.IndexName}@{x.PropertyName}");
        }

        // Per-attribute iteration. No PK dedup: a composite index can include a [PrimaryKey] property as one of its keys.
        private static IEnumerable<MetaIndexBinding> GetCompositeIndexBindings(IEnumerable<AbstractMetaItem> items)
        {
            return items.SelectMany(x =>
            {
                if (!x.Type.GetCustomAttributes<AbstractGraphItemAttribute>().Any()) return Enumerable.Empty<MetaIndexBinding>();

                var properties = x.Type.GetProperties().Where(y =>
                    y.GetCustomAttribute<PropertyKeyAttribute>(true) != null &&
                    y.GetCustomAttributes<CompositeIndexAttribute>().Any());

                return properties.SelectMany(y => y.GetCustomAttributes<CompositeIndexAttribute>().Select(attr => new MetaIndexBinding
                {
                    IndexName = Validate(attr.IndexName, "composite index binding name"),
                    PropertyName = Validate(y.Name, "property name")
                }));
            });
        }

        private static IEnumerable<MetaIndexBinding> GetPrimaryKeyBindings(IEnumerable<AbstractMetaItem> items)
        {
            return items.SelectMany(x =>
            {
                var properties = x.Type.GetProperties().Where(y =>
                    y.GetCustomAttribute<PropertyKeyAttribute>(true) != null &&
                    y.GetCustomAttribute<PrimaryKeyAttribute>() != null).ToList();
                if (properties.Count == 0) return Enumerable.Empty<MetaIndexBinding>();

                var graphItemAttribute = x.Type.GetCustomAttributes<AbstractGraphItemAttribute>().FirstOrDefault();
                if (graphItemAttribute == null) return Enumerable.Empty<MetaIndexBinding>();

                return properties.Select(y => new MetaIndexBinding
                {
                    IndexName = Validate(graphItemAttribute.Name ?? $"{x.Name}_PK", "primary key binding name"),
                    PropertyName = Validate(y.Name, "property name")
                });
            });
        }

        private static IEnumerable<MetaIndexBinding> GetAllMixedIndexBindings(IEnumerable<AbstractMetaItem> items)
        {
            return items.SelectMany(x =>
            {
                var properties = x.Type.GetProperties().Where(y =>
                    y.GetCustomAttribute<PropertyKeyAttribute>(true) != null &&
                    y.GetCustomAttributes<MixedIndexAttribute>().Any());

                if (!x.Type.GetCustomAttributes<AbstractGraphItemAttribute>().Any()) return Enumerable.Empty<MetaIndexBinding>();

                return properties.SelectMany(y =>
                    y.GetCustomAttributes<MixedIndexAttribute>().Select(attr => new MetaIndexBinding
                    {
                        IndexName = Validate(attr.IndexName, "mixed index binding name"),
                        PropertyName = Validate(y.Name, "property name"),
                        Mapping = attr.Mapping
                    })
                );
            });
        }
    }
}
