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
            return GetAllCompositeIndices<CompositeIndexAttribute>(schema.Vertices, ConvertCompositeAttribute)
                .Concat(GetAllCompositeIndices<PrimaryKeyAttribute>(schema.Vertices, ConvertPrimaryAttribute))
                .Concat(GetAllCompositeIndices<CompositeIndexAttribute>(schema.Edges.Where(x => x.Type != null), ConvertCompositeAttribute))
                .Concat(GetAllCompositeIndices<PrimaryKeyAttribute>(schema.Edges.Where(x => x.Type != null), ConvertPrimaryAttribute))
                .DistinctBy(x => x.Name);
        }

        private static MetaCompositeIndex ConvertCompositeAttribute(AbstractMetaItem graphItem, PropertyInfo property)
        {
            var compositeKeyAttribute = property.GetCustomAttribute<CompositeIndexAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on {property.Name}");

            return new MetaCompositeIndex
            {
                Name = Validate(compositeKeyAttribute.IndexName, "composite index name"),
                IsUnique = compositeKeyAttribute.IsUnique,
                IndexedElement = graphItem.Name,
                Kind = graphItem.Type.GetCustomAttribute<VertexAttribute>() != null ? "Vertex.class" : "Edge.class"
            };
        }

        private static MetaCompositeIndex ConvertPrimaryAttribute(AbstractMetaItem graphItem, PropertyInfo property)
        {
            var graphItemAttribute = graphItem.Type.GetCustomAttribute<AbstractGraphItemAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on {graphItem.Type.FullName}");

            return new MetaCompositeIndex
            {
                Name = Validate(graphItemAttribute.Name ?? $"{graphItem.Name}_PK", "primary key index name"),
                IsUnique = true,
                IndexedElement = graphItem.Name,
                Kind = graphItem.Type.GetCustomAttribute<VertexAttribute>() != null ? "Vertex.class" : "Edge.class"
            };
        }

        private static IEnumerable<MetaCompositeIndex> GetAllCompositeIndices<T>(IEnumerable<AbstractMetaItem> items, Func<AbstractMetaItem, PropertyInfo, MetaCompositeIndex> convert)
        {
            return items.SelectMany(x =>
            {
                var properties = x.Type.GetProperties().Where(y => y.GetCustomAttribute<PropertyKeyAttribute>(true) != null && y.GetCustomAttribute(typeof(T)) != null);

                return properties.Select(y => convert(x, y));
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
            return GetAllIndexBindings<CompositeIndexAttribute>(schema.Vertices, ConvertCompositeBinding)
                .Concat(GetAllIndexBindings<PrimaryKeyAttribute>(schema.Vertices, ConvertPrimaryBinding))
                .Concat(GetAllMixedIndexBindings(schema.Vertices))
                .Concat(GetAllIndexBindings<CompositeIndexAttribute>(schema.Edges.Where(x => x.Type != null), ConvertCompositeBinding))
                .Concat(GetAllIndexBindings<PrimaryKeyAttribute>(schema.Edges.Where(x => x.Type != null), ConvertPrimaryBinding))
                .Concat(GetAllMixedIndexBindings(schema.Edges.Where(x => x.Type != null)))
                .DistinctBy(x => $"{x.IndexName}@{x.PropertyName}");
        }

        private static IEnumerable<MetaIndexBinding> GetAllIndexBindings<T>(IEnumerable<AbstractMetaItem> items, Func<AbstractMetaItem, PropertyInfo, MetaIndexBinding> convert)
        {
            return items.SelectMany(x =>
            {
                var properties = x.Type.GetProperties().Where(y => y.GetCustomAttribute<PropertyKeyAttribute>(true) != null && y.GetCustomAttribute(typeof(T)) != null);

                if (!x.Type.GetCustomAttributes<AbstractGraphItemAttribute>().Any()) return new List<MetaIndexBinding>();

                return properties.Select(y => convert(x, y));
            });
        }

        private static MetaIndexBinding ConvertCompositeBinding(AbstractMetaItem graphItem, PropertyInfo property)
        {
            var compositeKeyAttribute = property.GetCustomAttribute<CompositeIndexAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on {property.Name}");

            return new MetaIndexBinding
            {
                IndexName = Validate(compositeKeyAttribute.IndexName, "composite index binding name"),
                PropertyName = Validate(property.Name, "property name")
            };
        }

        private static MetaIndexBinding ConvertPrimaryBinding(AbstractMetaItem graphItem, PropertyInfo property)
        {
            var graphItemAttribute = graphItem.Type.GetCustomAttribute<AbstractGraphItemAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on {graphItem.Type.FullName}");

            return new MetaIndexBinding
            {
                IndexName = Validate(graphItemAttribute.Name ?? $"{graphItem.Name}_PK", "primary key binding name"),
                PropertyName = Validate(property.Name, "property name")
            };
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
