using Ivet.Model;
using Ivet.Model.Library;
using Ivet.Model.Meta;
using Ivet.Services.Converters;
using Ivet.Tests.Types;
using Ivet.Tests.Types.Edges;
using Ivet.Tests.Types.Vertices;
using Xunit;

namespace Ivet.Tests.Services.Converters
{
    public class LibraryToSchemaConverterTests
    {
        [Fact]
        public void ConvertTest_Empty()
        {
            // Arrange
            var schema = new Schema();

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Connections);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Theory]
        [InlineData(typeof(VertexSample), "VertexSample", false, false)]
        [InlineData(typeof(NamedVertexSample), "A vertex name", false, false)]
        [InlineData(typeof(PartitionedVertexSample), "PartitionedVertexSample", true, false)]
        [InlineData(typeof(StaticVertexSample), "StaticVertexSample", false, true)]
        public void ConvertTest_Vertex(Type entityType, string name, bool isPartitioned, bool isStatic)
        {
            // Arrange
            var schema = new Schema
            {
                Vertices = { entityType }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Single(result.Vertices);
            Assert.Equal(name, result.Vertices[0].Name);
            Assert.Equal(isPartitioned, result.Vertices[0].Partitioned);
            Assert.Equal(isStatic, result.Vertices[0].Static);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Connections);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Theory]
        [InlineData(typeof(EdgeSample), "EdgeSample", typeof(VertexSample), typeof(NamedVertexSample), Multiplicity.SIMPLE)]
        [InlineData(typeof(NamedEdgeSample), "An edge name", typeof(VertexSample), typeof(NamedVertexSample), Multiplicity.SIMPLE)]
        [InlineData(typeof(InOutEdgeSample), "InOutEdgeSample", typeof(VertexSample), typeof(VertexSample), Multiplicity.SIMPLE)]
        [InlineData(typeof(MultiplicityEdgeSample), "MultiplicityEdgeSample", typeof(VertexSample), typeof(NamedVertexSample), Multiplicity.ONE2MANY)]
        public void ConvertTest_Edge(Type entityType, string name, Type inType, Type outType, Multiplicity multiplicity)
        {
            // Arrange
            var schema = new Schema
            {
                Edges = { entityType }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Single(result.Edges);
            Assert.Equal(name, result.Edges[0].Name);
            Assert.Equal(inType, result.Edges[0].In);
            Assert.Equal(outType, result.Edges[0].Out);
            Assert.Equal(multiplicity, result.Edges[0].Multiplicity);
            Assert.NotEmpty(result.Connections);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_EdgeDoubled()
        {
            // Arrange
            var schema = new Schema
            {
                Edges = { typeof(DoubleEdgeSample) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Equal(2, result.Edges.Count());
            Assert.Equal("DoubleEdgeSample", result.Edges[0].Name);
            Assert.Equal(typeof(VertexSample), result.Edges[0].In);
            Assert.Equal(typeof(StaticVertexSample), result.Edges[0].Out);
            Assert.Equal(Multiplicity.SIMPLE, result.Edges[0].Multiplicity);
            Assert.Equal("DoubleEdgeSample", result.Edges[1].Name);
            Assert.Equal(typeof(VertexSample), result.Edges[1].In);
            Assert.Equal(typeof(NamedVertexSample), result.Edges[1].Out);
            Assert.Equal(Multiplicity.SIMPLE, result.Edges[1].Multiplicity);
            Assert.NotEmpty(result.Connections);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Theory]
        [InlineData(typeof(EdgeSample), new Type[] { }, "EdgeSample", null, null)]
        [InlineData(typeof(EdgeSample), new Type[] { typeof(VertexSample) }, "EdgeSample", "VertexSample", null)]
        [InlineData(typeof(EdgeSample), new Type[] { typeof(NamedVertexSample) }, "EdgeSample", null, "A vertex name")]
        [InlineData(typeof(InOutEdgeSample), new Type[] { typeof(VertexSample) }, "InOutEdgeSample", "VertexSample", "VertexSample")]
        public void ConvertTest_Connections(Type edgeType, Type[] vertexTypes, string edgeName, string? inName, string? outName)
        {
            // Arrange
            var schema = new Schema
            {
                Edges = { edgeType },
                Vertices = vertexTypes.ToList()
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(vertexTypes.Length, result.Vertices.Count());
            Assert.NotEmpty(result.Edges);
            Assert.Single(result.Connections);
            Assert.Equal(edgeName, result.Connections[0].Edge);
            Assert.Equal(inName, result.Connections[0].Ingoing);
            Assert.Equal(outName, result.Connections[0].Outgoing);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_NoProperties()
        {
            // Arrange
            var schema = new Schema
            {
                Edges = { typeof(EdgeSample) },
                Vertices = { typeof(VertexSample) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Single(result.Vertices);
            Assert.Single(result.Edges);
            Assert.Single(result.Connections);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Theory]
        [InlineData(typeof(PropertyVertex), "MyProperty", "String.class", Cardinality.SINGLE)]
        [InlineData(typeof(NamedPropertyVertex), "My property", "String.class", Cardinality.SINGLE)]
        [InlineData(typeof(CardinalityPropertyVertex), "MyProperty", "String.class", Cardinality.LIST)]
        public void ConvertTest_VertexProperty(Type vertexType, string propertyName, string propertyType, Cardinality cardinality)
        {
            // Arrange
            var schema = new Schema
            {
                Vertices = { vertexType }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Connections);
            Assert.Single(result.Properties);
            Assert.Equal(propertyName, result.Properties[0].Name);
            Assert.Equal(propertyType, result.Properties[0].DataType);
            Assert.Equal(cardinality, result.Properties[0].Cardinality);
            Assert.NotNull(result.Properties[0].PropertyInfo);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.NotEmpty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_EdgeProperty()
        {
            // Arrange
            var schema = new Schema
            {
                Edges = { typeof(PropertyEdge) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.NotEmpty(result.Edges);
            Assert.NotEmpty(result.Connections);
            Assert.Equal(2, result.Properties.Count);
            Assert.Equal("MyProperty", result.Properties[0].Name);
            Assert.Equal("String.class", result.Properties[0].DataType);
            Assert.Equal(Cardinality.SINGLE, result.Properties[0].Cardinality);
            Assert.NotNull(result.Properties[0].PropertyInfo);
            Assert.Equal("MyCharProperty", result.Properties[1].Name);
            Assert.Equal("Character.class", result.Properties[1].DataType);
            Assert.Equal(Cardinality.SINGLE, result.Properties[1].Cardinality);
            Assert.NotNull(result.Properties[1].PropertyInfo);
            Assert.NotEmpty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_PropertiesWithSameName()
        {
            // Arrange
            var schema = new Schema
            {
                Edges = { typeof(PropertyEdge) },
                Vertices = { typeof(PropertyVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Vertices);
            Assert.NotEmpty(result.Edges);
            Assert.NotEmpty(result.Connections);
            Assert.Equal(2, result.Properties.Count);
            Assert.Equal("MyProperty", result.Properties[0].Name);
            Assert.Equal("String.class", result.Properties[0].DataType);
            Assert.Equal(Cardinality.SINGLE, result.Properties[0].Cardinality);
            Assert.NotNull(result.Properties[0].PropertyInfo);
            Assert.Equal("MyCharProperty", result.Properties[1].Name);
            Assert.Equal("Character.class", result.Properties[1].DataType);
            Assert.Equal(Cardinality.SINGLE, result.Properties[1].Cardinality);
            Assert.NotNull(result.Properties[1].PropertyInfo);
            Assert.NotEmpty(result.EdgePropertyBindings);
            Assert.NotEmpty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Theory]
        [InlineData(typeof(PropertyEdge), "PropertyEdge", "MyProperty")]
        public void ConvertTest_EdgePropertyBinding(Type edgeType, string entity, string property)
        {
            // Arrange
            var schema = new Schema
            {
                Edges = { edgeType }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.NotEmpty(result.Edges);
            Assert.NotEmpty(result.Connections);
            Assert.NotEmpty(result.Properties);
            Assert.Equal(2, result.EdgePropertyBindings.Count);
            Assert.Equal(entity, result.EdgePropertyBindings[0].Entity);
            Assert.Equal(property, result.EdgePropertyBindings[0].Name);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Theory]
        [InlineData(typeof(PropertyVertex), "PropertyVertex", "MyProperty")]
        public void ConvertTest_VertexPropertyBinding(Type vertexType, string entity, string property)
        {
            // Arrange
            var schema = new Schema
            {
                Vertices = { vertexType }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Connections);
            Assert.NotEmpty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Single(result.VertexPropertyBindings);
            Assert.Equal(entity, result.VertexPropertyBindings[0].Entity);
            Assert.Equal(property, result.VertexPropertyBindings[0].Name);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_PropertyBindingsWithSameName()
        {
            // Arrange
            var schema = new Schema
            {
                Edges = { typeof(PropertyEdge) },
                Vertices = { typeof(PropertyVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Vertices);
            Assert.NotEmpty(result.Edges);
            Assert.NotEmpty(result.Connections);
            Assert.NotEmpty(result.Properties);
            Assert.NotNull(result.Properties[0].PropertyInfo);
            Assert.Equal(2, result.EdgePropertyBindings.Count);
            Assert.Equal("PropertyEdge", result.EdgePropertyBindings[0].Entity);
            Assert.Equal("MyProperty", result.EdgePropertyBindings[0].Name);
            Assert.Equal("MyCharProperty", result.Properties[1].Name);
            Assert.Equal("Character.class", result.Properties[1].DataType);
            Assert.Single(result.VertexPropertyBindings);
            Assert.Equal("PropertyVertex", result.VertexPropertyBindings[0].Entity);
            Assert.Equal("MyProperty", result.VertexPropertyBindings[0].Name);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_EnumProperty()
        {
            // Arrange
            var schema = new Schema
            {
                Vertices = { typeof(PropertyEnumVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Connections);
            Assert.NotEmpty(result.Properties);
            Assert.NotNull(result.Properties[0].PropertyInfo);
            Assert.Equal("Property", result.Properties[0].Name);
            Assert.Equal("Integer.class", result.Properties[0].DataType);
            Assert.Single(result.VertexPropertyBindings);
            Assert.Equal("PropertyEnumVertex", result.VertexPropertyBindings[0].Entity);
            Assert.Equal("Property", result.VertexPropertyBindings[0].Name);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_StringEnumProperty()
        {
            // Arrange
            var schema = new Schema
            {
                Vertices = { typeof(StringPropertyEnumVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Connections);
            Assert.NotEmpty(result.Properties);
            Assert.NotNull(result.Properties[0].PropertyInfo);
            Assert.Equal("Property", result.Properties[0].Name);
            Assert.Equal("String.class", result.Properties[0].DataType);
            Assert.Single(result.VertexPropertyBindings);
            Assert.Equal("StringPropertyEnumVertex", result.VertexPropertyBindings[0].Entity);
            Assert.Equal("Property", result.VertexPropertyBindings[0].Name);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_CompositeIndex()
        {
            // Arrange
            var schema = new Schema
            {
                Vertices = { typeof(CompositeIndexVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            Assert.Single(result.CompositeIndexes);
            Assert.Equal("composite_idx", result.CompositeIndexes[0].Name);
            Assert.True(result.CompositeIndexes[0].IsUnique);
            Assert.Equal("CompositeIndexVertex", result.CompositeIndexes[0].IndexedElement);
            Assert.Equal("Vertex.class", result.CompositeIndexes[0].Kind);
            Assert.Single(result.IndexBindings);
            Assert.Equal("composite_idx", result.IndexBindings[0].IndexName);
            Assert.Equal("IndexedProperty", result.IndexBindings[0].PropertyName);
        }

        [Fact]
        public void ConvertTest_MixedIndex()
        {
            // Arrange
            var schema = new Schema
            {
                Vertices = { typeof(MixedIndexVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            Assert.Single(result.MixedIndexes);
            Assert.Equal("mixed_idx", result.MixedIndexes[0].Name);
            Assert.Equal("search", result.MixedIndexes[0].BackendIndex);
            Assert.Equal("MixedIndexVertex", result.MixedIndexes[0].IndexedElement);
            Assert.Equal("Vertex.class", result.MixedIndexes[0].Kind);
            Assert.Single(result.IndexBindings);
            Assert.Equal("mixed_idx", result.IndexBindings[0].IndexName);
            Assert.Equal("SearchProperty", result.IndexBindings[0].PropertyName);
            Assert.Equal(MappingType.TEXT, result.IndexBindings[0].Mapping);
        }

        [Fact]
        public void ConvertTest_PrimaryKey()
        {
            // Arrange
            var schema = new Schema
            {
                Vertices = { typeof(PrimaryKeyVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            Assert.Single(result.CompositeIndexes);
            Assert.Equal("PrimaryKeyVertex_PK", result.CompositeIndexes[0].Name);
            Assert.True(result.CompositeIndexes[0].IsUnique);
            Assert.Equal("PrimaryKeyVertex", result.CompositeIndexes[0].IndexedElement);
            Assert.Equal("Vertex.class", result.CompositeIndexes[0].Kind);
            Assert.Single(result.IndexBindings);
            Assert.Equal("PrimaryKeyVertex_PK", result.IndexBindings[0].IndexName);
            Assert.Equal("Id", result.IndexBindings[0].PropertyName);
        }

        [Fact]
        public void ConvertTest_PrimaryKey_AndCompositeIndex_OnSameProperty_KeepsBoth()
        {
            // Arrange — a property carrying both [PrimaryKey] and [CompositeIndex] is allowed: the user may want
            // a separate index (e.g., a multi-property composite that happens to include the PK property as one key).
            // The converter must NOT silently drop one. The user is responsible for declaring distinct index names.
            var schema = new Schema
            {
                Vertices = { typeof(PrimaryKeyAndCompositeIndexVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            Assert.Equal(2, result.CompositeIndexes.Count);
            Assert.Contains(result.CompositeIndexes, x => x.Name == "PrimaryKeyAndCompositeIndexVertex_PK" && x.IsUnique);
            Assert.Contains(result.CompositeIndexes, x => x.Name == "redundant_idx" && !x.IsUnique);
            Assert.Equal(2, result.IndexBindings.Count);
            Assert.Contains(result.IndexBindings, x => x.IndexName == "PrimaryKeyAndCompositeIndexVertex_PK" && x.PropertyName == "Id");
            Assert.Contains(result.IndexBindings, x => x.IndexName == "redundant_idx" && x.PropertyName == "Id");
        }

        [Fact]
        public void ConvertTest_PrimaryKey_AndCompositeIndex_OnDifferentProperties_KeepsBoth()
        {
            var schema = new Schema
            {
                Vertices = { typeof(PrimaryKeyAndDistinctCompositeIndexVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            Assert.Equal(2, result.CompositeIndexes.Count);
            Assert.Contains(result.CompositeIndexes, x => x.Name == "PrimaryKeyAndDistinctCompositeIndexVertex_PK" && x.IsUnique);
            Assert.Contains(result.CompositeIndexes, x => x.Name == "other_idx" && !x.IsUnique);
            Assert.Equal(2, result.IndexBindings.Count);
            Assert.Contains(result.IndexBindings, x => x.IndexName == "PrimaryKeyAndDistinctCompositeIndexVertex_PK" && x.PropertyName == "Id");
            Assert.Contains(result.IndexBindings, x => x.IndexName == "other_idx" && x.PropertyName == "OtherProperty");
        }

        [Fact]
        public void ConvertTest_PrimaryKey_AndMultipleCompositeIndex_OnSameProperty_KeepsAll()
        {
            // Arrange — multiple [CompositeIndex] piled on a [PrimaryKey] property must all be generated
            var schema = new Schema
            {
                Vertices = { typeof(PrimaryKeyAndMultipleCompositeIndexVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            Assert.Equal(3, result.CompositeIndexes.Count);
            Assert.Contains(result.CompositeIndexes, x => x.Name == "PrimaryKeyAndMultipleCompositeIndexVertex_PK" && x.IsUnique);
            Assert.Contains(result.CompositeIndexes, x => x.Name == "first_redundant");
            Assert.Contains(result.CompositeIndexes, x => x.Name == "second_redundant");
            Assert.Equal(3, result.IndexBindings.Count);
        }

        [Fact]
        public void ConvertTest_CompositeIndex_SharedAcrossPkAndNonPkProperties_BindsBoth()
        {
            // Arrange — a [CompositeIndex("multi_idx")] declared on both a [PrimaryKey] property and a non-PK
            // property describes a multi-property composite index. Both bindings must be generated, and the
            // index itself must appear once (deduped by name).
            var schema = new Schema
            {
                Vertices = { typeof(MultiPropertyCompositeWithPkVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            // PK index + the shared multi-property composite
            Assert.Equal(2, result.CompositeIndexes.Count);
            Assert.Contains(result.CompositeIndexes, x => x.Name == "MultiPropertyCompositeWithPkVertex_PK" && x.IsUnique);
            Assert.Contains(result.CompositeIndexes, x => x.Name == "multi_idx" && !x.IsUnique);

            // 3 bindings: PK→PkProp, multi_idx→PkProp, multi_idx→OtherProp
            Assert.Equal(3, result.IndexBindings.Count);
            Assert.Contains(result.IndexBindings, x => x.IndexName == "MultiPropertyCompositeWithPkVertex_PK" && x.PropertyName == "PkProp");
            Assert.Contains(result.IndexBindings, x => x.IndexName == "multi_idx" && x.PropertyName == "PkProp");
            Assert.Contains(result.IndexBindings, x => x.IndexName == "multi_idx" && x.PropertyName == "OtherProp");
        }

        [Fact]
        public void ConvertTest_MultipleCompositeIndex_OnSameProperty_GeneratesAll()
        {
            // Arrange — [CompositeIndex] is AllowMultiple = true; piling several on a property must yield N indexes,
            // not throw AmbiguousMatchException (regression for the singular GetCustomAttribute<> path)
            var schema = new Schema
            {
                Vertices = { typeof(MultiCompositeIndexVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            Assert.Equal(2, result.CompositeIndexes.Count);
            Assert.Contains(result.CompositeIndexes, x => x.Name == "first_idx" && !x.IsUnique && x.IndexedElement == "MultiCompositeIndexVertex");
            Assert.Contains(result.CompositeIndexes, x => x.Name == "second_idx" && x.IsUnique && x.IndexedElement == "MultiCompositeIndexVertex");
            Assert.Equal(2, result.IndexBindings.Count);
            Assert.Contains(result.IndexBindings, x => x.IndexName == "first_idx" && x.PropertyName == "IndexedProperty");
            Assert.Contains(result.IndexBindings, x => x.IndexName == "second_idx" && x.PropertyName == "IndexedProperty");
        }

        [Fact]
        public void ConvertTest_TwoPropertiesEachWithCompositeIndex_GeneratesAll()
        {
            // Arrange — distinct properties each with a single [CompositeIndex] (regression check that the new
            // refactor still iterates properly over multiple properties on the same vertex)
            var schema = new Schema
            {
                Vertices = { typeof(TwoPropertiesEachWithCompositeIndexVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            Assert.Equal(2, result.CompositeIndexes.Count);
            Assert.Contains(result.CompositeIndexes, x => x.Name == "idx_a" && !x.IsUnique);
            Assert.Contains(result.CompositeIndexes, x => x.Name == "idx_b" && x.IsUnique);
            Assert.Equal(2, result.IndexBindings.Count);
            Assert.Contains(result.IndexBindings, x => x.IndexName == "idx_a" && x.PropertyName == "PropA");
            Assert.Contains(result.IndexBindings, x => x.IndexName == "idx_b" && x.PropertyName == "PropB");
        }

        [Fact]
        public void ConvertTest_Edge_PrimaryKey_Standalone()
        {
            // Arrange — [PrimaryKey] on an edge property (existing test only covered vertex)
            var schema = new Schema
            {
                Vertices = { typeof(VertexSample) },
                Edges = { typeof(PrimaryKeyEdge) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            Assert.Single(result.CompositeIndexes);
            Assert.Equal("PrimaryKeyEdge_PK", result.CompositeIndexes[0].Name);
            Assert.True(result.CompositeIndexes[0].IsUnique);
            Assert.Equal("PrimaryKeyEdge", result.CompositeIndexes[0].IndexedElement);
            Assert.Equal("Edge.class", result.CompositeIndexes[0].Kind);
            Assert.Single(result.IndexBindings);
            Assert.Equal("PrimaryKeyEdge_PK", result.IndexBindings[0].IndexName);
            Assert.Equal("Id", result.IndexBindings[0].PropertyName);
        }

        [Fact]
        public void ConvertTest_Edge_MultipleCompositeIndex_OnSameProperty_GeneratesAll()
        {
            // Arrange — multi-CompositeIndex on edge property (mirrors the vertex case)
            var schema = new Schema
            {
                Vertices = { typeof(VertexSample) },
                Edges = { typeof(MultiCompositeIndexEdge) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            Assert.Equal(2, result.CompositeIndexes.Count);
            Assert.Contains(result.CompositeIndexes, x => x.Name == "edge_first" && !x.IsUnique && x.Kind == "Edge.class");
            Assert.Contains(result.CompositeIndexes, x => x.Name == "edge_second" && x.IsUnique && x.Kind == "Edge.class");
            Assert.Equal(2, result.IndexBindings.Count);
        }

        [Fact]
        public void ConvertTest_Edge_PrimaryKey_AndCompositeIndex_OnSameProperty_KeepsBoth()
        {
            // Arrange — edges follow the same rule: PK and CompositeIndex coexist on the same property
            var schema = new Schema
            {
                Vertices = { typeof(VertexSample) },
                Edges = { typeof(PrimaryKeyAndCompositeIndexEdge) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            Assert.Equal(2, result.CompositeIndexes.Count);
            Assert.Contains(result.CompositeIndexes, x => x.Name == "PrimaryKeyAndCompositeIndexEdge_PK" && x.IsUnique && x.Kind == "Edge.class");
            Assert.Contains(result.CompositeIndexes, x => x.Name == "redundant_edge_idx" && x.Kind == "Edge.class");
            Assert.Equal(2, result.IndexBindings.Count);
            Assert.Contains(result.IndexBindings, x => x.IndexName == "PrimaryKeyAndCompositeIndexEdge_PK" && x.PropertyName == "Id");
            Assert.Contains(result.IndexBindings, x => x.IndexName == "redundant_edge_idx" && x.PropertyName == "Id");
        }

        [Fact]
        public void ConvertTest_MixedIndex_SharedAcrossVertices_BecomesGlobal()
        {
            // Arrange — two vertex types inherit the same [MixedIndex] properties from a shared base
            var schema = new Schema
            {
                Vertices = { typeof(SharedMixedIndexVertexA), typeof(SharedMixedIndexVertexB) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert — single index with empty IndexedElement (global, no indexOnly)
            Assert.Single(result.MixedIndexes);
            Assert.Equal("shared_idx", result.MixedIndexes[0].Name);
            Assert.Equal("search", result.MixedIndexes[0].BackendIndex);
            Assert.Equal(string.Empty, result.MixedIndexes[0].IndexedElement);
            Assert.Equal("Vertex.class", result.MixedIndexes[0].Kind);

            // Two index bindings (SharedName + SharedCode), deduplicated across vertex types
            var mixedBindings = result.IndexBindings.Where(b => b.IndexName == "shared_idx").ToList();
            Assert.Equal(2, mixedBindings.Count);
            Assert.Contains(mixedBindings, b => b.PropertyName == "SharedName" && b.Mapping == MappingType.TEXT);
            Assert.Contains(mixedBindings, b => b.PropertyName == "SharedCode" && b.Mapping == MappingType.STRING);
        }

        [Fact]
        public void ConvertTest_MixedIndex_SingleVertex_UsesIndexOnly()
        {
            // Arrange — single vertex type → should keep IndexedElement (indexOnly)
            var schema = new Schema
            {
                Vertices = { typeof(MixedIndexVertex) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert — IndexedElement is set (indexOnly will be generated)
            Assert.Single(result.MixedIndexes);
            Assert.Equal("MixedIndexVertex", result.MixedIndexes[0].IndexedElement);
        }

        [Fact]
        public void ConvertTest_EdgePropertyAttribute()
        {
            // Arrange
            var schema = new Schema
            {
                Vertices = { typeof(EdgePropertyVertex), typeof(VertexSample) }
            };

            // Act
            var result = LibraryToSchemaConverter.Convert(schema);

            // Assert
            Assert.Equal(2, result.Vertices.Count());
            Assert.Single(result.Edges);
            Assert.Equal("EdgePropertyVertex_Related", result.Edges[0].Name);
            Assert.Equal(typeof(EdgePropertyVertex), result.Edges[0].In);
            Assert.Equal(typeof(VertexSample), result.Edges[0].Out);
            Assert.Single(result.Connections);
        }
    }
}
