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

        [Theory]
        [InlineData(typeof(EdgeSample), new Type[] { }, "EdgeSample", null, null)]
        [InlineData(typeof(EdgeSample), new Type[] { typeof(VertexSample) }, "EdgeSample", "VertexSample", null)]
        [InlineData(typeof(EdgeSample), new Type[] { typeof(NamedVertexSample)  }, "EdgeSample", null, "A vertex name")]
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
    }
}
