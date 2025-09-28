using Ivet.Model;
using Ivet.Model.Library;
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
    }
}
