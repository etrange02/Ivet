using Ivet.Extensions;
using Ivet.Model;
using Ivet.Model.Meta;
using Ivet.Services;
using Ivet.TestFramework;
using Xunit;

namespace Ivet.Tests.Services
{
    public class MigrationBuilderTests
    {
        RandomGenerator _randomGenerator = new RandomGenerator();

        [Fact]
        public void BuildTest_EmptySchema()
        {
            // Arrange
            var sut = new MigrationBuilder { MetaSchema = new MetaSchema() };
            sut.MetaSchema = new MetaSchema
            {

            };

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Empty(result[0]);
        }

        [Fact]
        public void BuildTest_Vertex()
        {
            // Arrange
            var sut = new MigrationBuilder { MetaSchema = new MetaSchema() };
            var name = _randomGenerator.RandomString();
            sut.MetaSchema.Vertices.Add(new MetaVertex
            {
                Name = name
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices\r\nmgmt.makeVertexLabel('{name}').make();\r\n// Edges\r\n\r\n// Properties\r\n\r\n// Vertex property bindings\r\n\r\n// Edge property bindings\r\n\r\n// Connections\r\n\r\n", result[0]);
        }

        [Fact]
        public void BuildTest_VertexPartitioned()
        {
            // Arrange
            var sut = new MigrationBuilder { MetaSchema = new MetaSchema() };
            var name = _randomGenerator.RandomString();
            var partitioned = _randomGenerator.RandomBool();
            var staticValue = _randomGenerator.RandomBool();
            sut.MetaSchema.Vertices.Add(new MetaVertex
            {
                Name = name,
                Partitioned = true
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices\r\nmgmt.makeVertexLabel('{name}').partition().make();\r\n// Edges\r\n\r\n// Properties\r\n\r\n// Vertex property bindings\r\n\r\n// Edge property bindings\r\n\r\n// Connections\r\n\r\n", result[0]);
        }

        [Fact]
        public void BuildTest_VertexStatic()
        {
            // Arrange
            var sut = new MigrationBuilder { MetaSchema = new MetaSchema() };
            var name = _randomGenerator.RandomString();
            sut.MetaSchema.Vertices.Add(new MetaVertex
            {
                Name = name,
                Static = true
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices\r\nmgmt.makeVertexLabel('{name}').setStatic().make();\r\n// Edges\r\n\r\n// Properties\r\n\r\n// Vertex property bindings\r\n\r\n// Edge property bindings\r\n\r\n// Connections\r\n\r\n", result[0]);
        }

        [Fact]
        public void BuildTest_VertexStaticPartitioned()
        {
            // Arrange
            var sut = new MigrationBuilder { MetaSchema = new MetaSchema() };
            var name = _randomGenerator.RandomString();
            sut.MetaSchema.Vertices.Add(new MetaVertex
            {
                Name = name,
                Partitioned = true,
                Static = true
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices\r\nmgmt.makeVertexLabel('{name}').partition().setStatic().make();\r\n// Edges\r\n\r\n// Properties\r\n\r\n// Vertex property bindings\r\n\r\n// Edge property bindings\r\n\r\n// Connections\r\n\r\n", result[0]);
        }

        [Fact]
        public void BuildTest_Edge()
        {
            // Arrange
            var sut = new MigrationBuilder { MetaSchema = new MetaSchema() };
            var name = _randomGenerator.RandomString();
            var multiplicity = _randomGenerator.RandomEnum<Multiplicity>();
            sut.MetaSchema.Edges.Add(new MetaEdge
            {
                Name = name,
                Multiplicity = multiplicity
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices\r\n\r\n// Edges\r\nmgmt.makeEdgeLabel('{ name }').multiplicity({ multiplicity.ToJavaString() }).make();\r\n// Properties\r\n\r\n// Vertex property bindings\r\n\r\n// Edge property bindings\r\n\r\n// Connections\r\n\r\n", result[0]);
        }

        [Fact]
        public void BuildTest_Property()
        {
            // Arrange
            var sut = new MigrationBuilder { MetaSchema = new MetaSchema() };
            var name = _randomGenerator.RandomString();
            var cardinality = _randomGenerator.RandomEnum<Cardinality>();
            var dataType = _randomGenerator.RandomString();
            sut.MetaSchema.Properties.Add(new MetaPropertyKey
            {
                Name = name,
                Cardinality = cardinality,
                DataType = dataType
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices\r\n\r\n// Edges\r\n\r\n// Properties\r\nmgmt.makePropertyKey('{ name }').dataType({ dataType }).cardinality({ cardinality.ToJavaString() }).make();\r\n// Vertex property bindings\r\n\r\n// Edge property bindings\r\n\r\n// Connections\r\n\r\n", result[0]);
        }

        [Fact]
        public void BuildTest_VertexPropertyBinding()
        {
            // Arrange
            var sut = new MigrationBuilder { MetaSchema = new MetaSchema() };
            var name = _randomGenerator.RandomString();
            var entity = _randomGenerator.RandomString();
            sut.MetaSchema.VertexPropertyBindings.Add(new MetaPropertyBinding
            {
                Name = name,
                Entity = entity
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices\r\n\r\n// Edges\r\n\r\n// Properties\r\n\r\n// Vertex property bindings\r\nvertex = mgmt.getVertexLabel('{ entity }');prop = mgmt.getPropertyKey('{ name }');mgmt.addProperties(vertex, prop);\r\n// Edge property bindings\r\n\r\n// Connections\r\n\r\n", result[0]);
        }

        [Fact]
        public void BuildTest_EdgePropertyBinding()
        {
            // Arrange
            var sut = new MigrationBuilder { MetaSchema = new MetaSchema() };
            var name = _randomGenerator.RandomString();
            var entity = _randomGenerator.RandomString();
            sut.MetaSchema.EdgePropertyBindings.Add(new MetaPropertyBinding
            {
                Name = name,
                Entity = entity
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices\r\n\r\n// Edges\r\n\r\n// Properties\r\n\r\n// Vertex property bindings\r\n\r\n// Edge property bindings\r\nedge = mgmt.getEdgeLabel('{ entity }');prop = mgmt.getPropertyKey('{ name }');mgmt.addProperties(edge, prop);\r\n// Connections\r\n\r\n", result[0]);
        }

        [Fact]
        public void BuildTest_Connection()
        {
            // Arrange
            var sut = new MigrationBuilder { MetaSchema = new MetaSchema() };
            var edge = _randomGenerator.RandomString();
            var ingoing = _randomGenerator.RandomString();
            var outgoing = _randomGenerator.RandomString();
            sut.MetaSchema.Connections.Add(new MetaConnection
            {
                Edge = edge,
                Ingoing = ingoing,
                Outgoing = outgoing
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices\r\n\r\n// Edges\r\n\r\n// Properties\r\n\r\n// Vertex property bindings\r\n\r\n// Edge property bindings\r\n\r\n// Connections\r\ninput = mgmt.getVertexLabel('{ ingoing }');output = mgmt.getVertexLabel('{ outgoing }');edge = mgmt.getEdgeLabel('{ edge }');mgmt.addConnection(edge, output, input);\r\n", result[0]);
        }
    }
}
