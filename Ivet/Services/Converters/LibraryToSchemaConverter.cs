using Ivet.Extensions;
using Ivet.Model;
using Ivet.Model.Library;
using Ivet.Model.Meta;
using System.Reflection;

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
                    Name = attribute.Name ?? x.Name,
                    Partitioned = attribute.Partitioned,
                    Static = attribute.Static,
                    Type = x,
                    Attribute = attribute
                };
            }).DistinctBy(x => x.Name);
        }

        private static IEnumerable<MetaEdge> GetEdges(Schema schema)
        {
            return schema.Edges.ConvertAll(x =>
            {
                var attribute = x.GetCustomAttribute<EdgeAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on {x.FullName}");
                return new MetaEdge
                {
                    Name = attribute.Name ?? x.Name,
                    Multiplicity = attribute.Multiplicity,
                    Type = x,
                    Attribute = attribute,
                    In = attribute.In,
                    Out = attribute.Out
                };

            }).DistinctBy(x => x.Name)
            .Concat(schema.Vertices.SelectMany(x =>
            {
                var properties = x.GetProperties().Where(y => y.GetCustomAttribute<EdgePropertyAttribute>() != null);

                return properties.Select(y =>
                {
                    var attribute = y.GetCustomAttribute<EdgePropertyAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on {x.FullName}");
                    var type = y.PropertyType.IsGenericType ? y.PropertyType.GenericTypeArguments[0] : y.PropertyType.GetElementType();
                    return new MetaEdge
                    {
                        Name = attribute.Name ?? $"{x.Name}_{y.Name}",
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
                var properties = x.GetProperties().Where(y => y.GetCustomAttribute<PropertyKeyAttribute>() != null);

                return properties.Select(y =>
                {
                    var attribute = y.GetCustomAttribute<PropertyKeyAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on {x.FullName}");
                    return new MetaPropertyKey
                    {
                        Name = attribute.Name ?? y.Name,
                        Cardinality = attribute.Cardinality,
                        DataType = y.ToJavaType(),
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
                var properties = x.Type.GetProperties().Where(y => y.GetCustomAttribute<PropertyKeyAttribute>() != null);

                return properties.Select(y =>
                {
                    var attribute = y.GetCustomAttribute<PropertyKeyAttribute>();
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
                Name = compositeKeyAttribute.IndexName,
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
                Name = graphItemAttribute.Name ?? $"{graphItem.Name}_PK",
                IsUnique = true,
                IndexedElement = graphItem.Name,
                Kind = graphItem.Type.GetCustomAttribute<VertexAttribute>() != null ? "Vertex.class" : "Edge.class"
            };
        }

        private static IEnumerable<MetaCompositeIndex> GetAllCompositeIndices<T>(IEnumerable<AbstractMetaItem> items, Func<AbstractMetaItem, PropertyInfo, MetaCompositeIndex> convert)
        {
            return items.SelectMany(x =>
            {
                var properties = x.Type.GetProperties().Where(y => y.GetCustomAttribute<PropertyKeyAttribute>() != null && y.GetCustomAttribute(typeof(T)) != null);

                return properties.Select(y => convert(x, y));
            });
        }

        private static IEnumerable<MetaMixedIndex> GetMixedIndices(Schema schema)
        {
            return schema.Vertices.Concat(schema.Edges).SelectMany(x =>
            {
                var properties = x.GetProperties().Where(y => y.GetCustomAttribute<PropertyKeyAttribute>() != null && y.GetCustomAttribute<MixedIndexAttribute>() != null);

                if (x.GetCustomAttribute<AbstractGraphItemAttribute>() == null) return new List<MetaMixedIndex>();

                return properties.Select(y =>
                {
                    var mixedKeyAttribute = y.GetCustomAttribute<MixedIndexAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on {x.FullName}");

                    return new MetaMixedIndex
                    {
                        Name = mixedKeyAttribute.IndexName,
                        BackendIndex = mixedKeyAttribute.Backend,
                        IndexedElement = x.Name,
                        Kind = x.GetCustomAttribute<VertexAttribute>() != null ? "Vertex.class" : "Edge.class"
                    };
                });
            }).DistinctBy(x => x.Name);
        }

        private static IEnumerable<MetaIndexBinding> GetIndexBindings(MetaSchema schema)
        {
            return GetAllIndexBindings<CompositeIndexAttribute>(schema.Vertices, ConvertCompositeBinding)
                .Concat(GetAllIndexBindings<PrimaryKeyAttribute>(schema.Vertices, ConvertPrimaryBinding))
                .Concat(GetAllIndexBindings<MixedIndexAttribute>(schema.Vertices, ConvertMixedBinding))
                .Concat(GetAllIndexBindings<CompositeIndexAttribute>(schema.Edges.Where(x => x.Type != null), ConvertCompositeBinding))
                .Concat(GetAllIndexBindings<PrimaryKeyAttribute>(schema.Edges.Where(x => x.Type != null), ConvertPrimaryBinding))
                .Concat(GetAllIndexBindings<MixedIndexAttribute>(schema.Edges.Where(x => x.Type != null), ConvertMixedBinding))
                .DistinctBy(x => $"{x.IndexName}@{x.PropertyName}");
        }

        private static IEnumerable<MetaIndexBinding> GetAllIndexBindings<T>(IEnumerable<AbstractMetaItem> items, Func<AbstractMetaItem, PropertyInfo, MetaIndexBinding> convert)
        {
            return items.SelectMany(x =>
            {
                var properties = x.Type.GetProperties().Where(y => y.GetCustomAttribute<PropertyKeyAttribute>() != null && y.GetCustomAttribute(typeof(T)) != null);

                if (x.Type.GetCustomAttribute<AbstractGraphItemAttribute>() == null) return new List<MetaIndexBinding>();

                return properties.Select(y => convert(x, y));
            });
        }

        private static MetaIndexBinding ConvertCompositeBinding(AbstractMetaItem graphItem, PropertyInfo property)
        {
            var compositeKeyAttribute = property.GetCustomAttribute<CompositeIndexAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on {property.Name}");

            return new MetaIndexBinding
            {
                IndexName = compositeKeyAttribute.IndexName,
                PropertyName = property.Name
            };
        }

        private static MetaIndexBinding ConvertPrimaryBinding(AbstractMetaItem graphItem, PropertyInfo property)
        {
            var graphItemAttribute = graphItem.Type.GetCustomAttribute<AbstractGraphItemAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on {graphItem.Type.FullName}");

            return new MetaIndexBinding
            {
                IndexName = graphItemAttribute.Name ?? $"{graphItem.Name}_PK",
                PropertyName = property.Name
            };
        }

        private static MetaIndexBinding ConvertMixedBinding(AbstractMetaItem graphItem, PropertyInfo property)
        {
            var mixedKeyAttribute = property.GetCustomAttribute<MixedIndexAttribute>() ?? throw new AttributeNotFoundException($"Attribute not found on {property.Name}");

            return new MetaIndexBinding
            {
                IndexName = mixedKeyAttribute.IndexName,
                PropertyName = property.Name,
                Mapping = mixedKeyAttribute.Mapping
            };
        }
    }
}
