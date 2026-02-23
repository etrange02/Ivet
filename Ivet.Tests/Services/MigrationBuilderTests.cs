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
        [Fact]
        public void BuildTest_EmptySchema()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());

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
            var sut = new MigrationBuilder(new MetaSchema());
            var name = RandomGenerator.RandomString();
            sut.MetaSchema.Vertices.Add(new MetaVertex
            {
                Name = name
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices{ Environment.NewLine }mgmt.makeVertexLabel('{name}').make();{ Environment.NewLine }// Edges{ Environment.NewLine }{ Environment.NewLine }// Properties{ Environment.NewLine }{ Environment.NewLine }// Vertex property bindings{ Environment.NewLine }{ Environment.NewLine }// Edge property bindings{ Environment.NewLine }{ Environment.NewLine }// Connections{ Environment.NewLine }{ Environment.NewLine }", result[0]);
        }

        [Fact]
        public void BuildTest_VertexPartitioned()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var name = RandomGenerator.RandomString();
            var partitioned = RandomGenerator.RandomBool();
            var staticValue = RandomGenerator.RandomBool();
            sut.MetaSchema.Vertices.Add(new MetaVertex
            {
                Name = name,
                Partitioned = true
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices{ Environment.NewLine }mgmt.makeVertexLabel('{name}').partition().make();{ Environment.NewLine }// Edges{ Environment.NewLine }{ Environment.NewLine }// Properties{ Environment.NewLine }{ Environment.NewLine }// Vertex property bindings{ Environment.NewLine }{ Environment.NewLine }// Edge property bindings{ Environment.NewLine }{ Environment.NewLine }// Connections{ Environment.NewLine }{ Environment.NewLine }", result[0]);
        }

        [Fact]
        public void BuildTest_VertexStatic()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var name = RandomGenerator.RandomString();
            sut.MetaSchema.Vertices.Add(new MetaVertex
            {
                Name = name,
                Static = true
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices{ Environment.NewLine }mgmt.makeVertexLabel('{name}').setStatic().make();{ Environment.NewLine }// Edges{ Environment.NewLine }{ Environment.NewLine }// Properties{ Environment.NewLine }{ Environment.NewLine }// Vertex property bindings{ Environment.NewLine }{ Environment.NewLine }// Edge property bindings{ Environment.NewLine }{ Environment.NewLine }// Connections{ Environment.NewLine }{ Environment.NewLine }", result[0]);
        }

        [Fact]
        public void BuildTest_VertexStaticPartitioned()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var name = RandomGenerator.RandomString();
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
            Assert.Equal($"// Vertices{ Environment.NewLine }mgmt.makeVertexLabel('{name}').partition().setStatic().make();{ Environment.NewLine }// Edges{ Environment.NewLine }{ Environment.NewLine }// Properties{ Environment.NewLine }{ Environment.NewLine }// Vertex property bindings{ Environment.NewLine }{ Environment.NewLine }// Edge property bindings{ Environment.NewLine }{ Environment.NewLine }// Connections{ Environment.NewLine }{ Environment.NewLine }", result[0]);
        }

        [Fact]
        public void BuildTest_Edge()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var name = RandomGenerator.RandomString();
            var multiplicity = RandomGenerator.RandomEnum<Multiplicity>();
            sut.MetaSchema.Edges.Add(new MetaEdge
            {
                Name = name,
                Multiplicity = multiplicity
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices{ Environment.NewLine }{ Environment.NewLine }// Edges{ Environment.NewLine }mgmt.makeEdgeLabel('{ name }').multiplicity({ multiplicity.ToJavaString() }).make();{ Environment.NewLine }// Properties{ Environment.NewLine }{ Environment.NewLine }// Vertex property bindings{ Environment.NewLine }{ Environment.NewLine }// Edge property bindings{ Environment.NewLine }{ Environment.NewLine }// Connections{ Environment.NewLine }{ Environment.NewLine }", result[0]);
        }

        [Fact]
        public void BuildTest_Property()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var name = RandomGenerator.RandomString();
            var cardinality = RandomGenerator.RandomEnum<Cardinality>();
            var dataType = RandomGenerator.RandomString();
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
            Assert.Equal($"// Vertices{ Environment.NewLine }{ Environment.NewLine }// Edges{ Environment.NewLine }{ Environment.NewLine }// Properties{ Environment.NewLine }mgmt.makePropertyKey('{ name }').dataType({ dataType }).cardinality({ cardinality.ToJavaString() }).make();{ Environment.NewLine }// Vertex property bindings{ Environment.NewLine }{ Environment.NewLine }// Edge property bindings{ Environment.NewLine }{ Environment.NewLine }// Connections{ Environment.NewLine }{ Environment.NewLine }", result[0]);
        }

        [Fact]
        public void BuildTest_VertexPropertyBinding()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var name = RandomGenerator.RandomString();
            var entity = RandomGenerator.RandomString();
            sut.MetaSchema.VertexPropertyBindings.Add(new MetaPropertyBinding
            {
                Name = name,
                Entity = entity
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices{ Environment.NewLine }{ Environment.NewLine }// Edges{ Environment.NewLine }{ Environment.NewLine }// Properties{ Environment.NewLine }{ Environment.NewLine }// Vertex property bindings{ Environment.NewLine }vertex = mgmt.getVertexLabel('{ entity }');prop = mgmt.getPropertyKey('{ name }');mgmt.addProperties(vertex, prop);{ Environment.NewLine }// Edge property bindings{ Environment.NewLine }{ Environment.NewLine }// Connections{ Environment.NewLine }{ Environment.NewLine }", result[0]);
        }

        [Fact]
        public void BuildTest_EdgePropertyBinding()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var name = RandomGenerator.RandomString();
            var entity = RandomGenerator.RandomString();
            sut.MetaSchema.EdgePropertyBindings.Add(new MetaPropertyBinding
            {
                Name = name,
                Entity = entity
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
            Assert.Equal($"// Vertices{ Environment.NewLine }{ Environment.NewLine }// Edges{ Environment.NewLine }{ Environment.NewLine }// Properties{ Environment.NewLine }{ Environment.NewLine }// Vertex property bindings{ Environment.NewLine }{ Environment.NewLine }// Edge property bindings{ Environment.NewLine }edge = mgmt.getEdgeLabel('{ entity }');prop = mgmt.getPropertyKey('{ name }');mgmt.addProperties(edge, prop);{ Environment.NewLine }// Connections{ Environment.NewLine }{ Environment.NewLine }", result[0]);
        }

        [Fact]
        public void BuildTest_Connection()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var edge = RandomGenerator.RandomString();
            var ingoing = RandomGenerator.RandomString();
            var outgoing = RandomGenerator.RandomString();
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
            Assert.Equal($"// Vertices{ Environment.NewLine }{ Environment.NewLine }// Edges{ Environment.NewLine }{ Environment.NewLine }// Properties{ Environment.NewLine }{ Environment.NewLine }// Vertex property bindings{ Environment.NewLine }{ Environment.NewLine }// Edge property bindings{ Environment.NewLine }{ Environment.NewLine }// Connections{ Environment.NewLine }input = mgmt.getVertexLabel('{ ingoing }');output = mgmt.getVertexLabel('{ outgoing }');edge = mgmt.getEdgeLabel('{ edge }');mgmt.addConnection(edge, output, input);{ Environment.NewLine }", result[0]);
        }

        [Fact]
        public void BuildTest_CompositeIndex()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var indexName = RandomGenerator.RandomString();
            var indexedElement = RandomGenerator.RandomString();
            var kind = RandomGenerator.RandomString();
            var propertyName = RandomGenerator.RandomString();
            sut.MetaSchema!.CompositeIndexes.Add(new MetaCompositeIndex
            {
                Name = indexName,
                IsUnique = true,
                IndexedElement = indexedElement,
                Kind = kind
            });
            sut.MetaSchema.IndexBindings.Add(new MetaIndexBinding
            {
                IndexName = indexName,
                PropertyName = propertyName
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains($"// Composite Indices", result[1]);
            Assert.Contains($"mgmt.buildIndex('{indexName}', {kind}).indexOnly(vertex).unique()", result[1]);
            Assert.Contains($".buildCompositeIndex();", result[1]);
            Assert.Contains($"prop_0 = mgmt.getPropertyKey('{propertyName}');", result[1]);
            Assert.Contains($"ManagementSystem.awaitGraphIndexStatus(graph, '{indexName}').call();", result[1]);
            Assert.Contains($"mgmt.updateIndex(mgmt.getGraphIndex('{indexName}'), SchemaAction.REINDEX).get();", result[1]);
        }

        [Fact]
        public void BuildTest_CompositeIndex_NotUnique()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var indexName = RandomGenerator.RandomString();
            sut.MetaSchema!.CompositeIndexes.Add(new MetaCompositeIndex
            {
                Name = indexName,
                IsUnique = false,
                IndexedElement = RandomGenerator.RandomString(),
                Kind = RandomGenerator.RandomString()
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.DoesNotContain(".unique()", result[1]);
        }

        [Fact]
        public void BuildTest_MixedIndex()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var indexName = RandomGenerator.RandomString();
            var indexedElement = RandomGenerator.RandomString();
            var kind = RandomGenerator.RandomString();
            var backend = RandomGenerator.RandomString();
            var propertyName = RandomGenerator.RandomString();
            sut.MetaSchema!.MixedIndexes.Add(new MetaMixedIndex
            {
                Name = indexName,
                IndexedElement = indexedElement,
                Kind = kind,
                BackendIndex = backend
            });
            sut.MetaSchema.IndexBindings.Add(new MetaIndexBinding
            {
                IndexName = indexName,
                PropertyName = propertyName,
                Mapping = MappingType.TEXT
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains($"// Mixed Indices", result[1]);
            Assert.Contains($"mgmt.buildIndex('{indexName}', {kind}).indexOnly(vertex)", result[1]);
            Assert.Contains($".buildMixedIndex('{backend}');", result[1]);
            Assert.Contains($"prop_0 = mgmt.getPropertyKey('{propertyName}');", result[1]);
            Assert.Contains($".addKey(prop_0, Mapping.TEXT.asParameter())", result[1]);
            Assert.Contains($"ManagementSystem.awaitGraphIndexStatus(graph, '{indexName}').call();", result[1]);
            Assert.Contains($"mgmt.updateIndex(mgmt.getGraphIndex('{indexName}'), SchemaAction.REINDEX).get();", result[1]);
        }

        [Fact]
        public void BuildTest_MixedIndex_NoMapping()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var indexName = RandomGenerator.RandomString();
            var propertyName = RandomGenerator.RandomString();
            sut.MetaSchema!.MixedIndexes.Add(new MetaMixedIndex
            {
                Name = indexName,
                IndexedElement = RandomGenerator.RandomString(),
                Kind = RandomGenerator.RandomString(),
                BackendIndex = RandomGenerator.RandomString()
            });
            sut.MetaSchema.IndexBindings.Add(new MetaIndexBinding
            {
                IndexName = indexName,
                PropertyName = propertyName,
                Mapping = MappingType.NULL
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains($".addKey(prop_0)", result[1]);
            Assert.DoesNotContain("Mapping.", result[1]);
        }

        [Fact]
        public void BuildTest_MultipleCompositeIndexes()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var indexName1 = RandomGenerator.RandomString();
            var indexName2 = RandomGenerator.RandomString();
            sut.MetaSchema!.CompositeIndexes.Add(new MetaCompositeIndex { Name = indexName1, IndexedElement = RandomGenerator.RandomString(), Kind = RandomGenerator.RandomString() });
            sut.MetaSchema.CompositeIndexes.Add(new MetaCompositeIndex { Name = indexName2, IndexedElement = RandomGenerator.RandomString(), Kind = RandomGenerator.RandomString() });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains($"ManagementSystem.awaitGraphIndexStatus(graph, '{indexName1}').call();", result[1]);
            Assert.Contains($"ManagementSystem.awaitGraphIndexStatus(graph, '{indexName2}').call();", result[1]);
            Assert.Contains($"mgmt.updateIndex(mgmt.getGraphIndex('{indexName1}'), SchemaAction.REINDEX).get();", result[1]);
            Assert.Contains($"mgmt.updateIndex(mgmt.getGraphIndex('{indexName2}'), SchemaAction.REINDEX).get();", result[1]);
        }

        [Fact]
        public void BuildTest_IndexBinding_Orphan()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            var indexName = RandomGenerator.RandomString();
            var propertyName = RandomGenerator.RandomString();
            sut.MetaSchema!.IndexBindings.Add(new MetaIndexBinding
            {
                IndexName = indexName,
                PropertyName = propertyName,
                Mapping = MappingType.TEXTSTRING
            });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains($"prop = mgmt.getPropertyKey('{propertyName}');index = mgmt.getGraphIndex('{indexName}').addKey(prop, Mapping.TEXTSTRING.asParameter());", result[1]);
            Assert.Contains($"ManagementSystem.awaitGraphIndexStatus(graph, '{indexName}').call();", result[1]);
            Assert.Contains($"mgmt.updateIndex(mgmt.getGraphIndex('{indexName}'), SchemaAction.REINDEX).get();", result[1]);
        }

        [Fact]
        public void BuildTest_NoIndexes_EmptyResult()
        {
            // Arrange
            var sut = new MigrationBuilder(new MetaSchema());
            sut.MetaSchema!.Vertices.Add(new MetaVertex { Name = RandomGenerator.RandomString() });

            // Act
            var result = sut.Build();

            // Assert
            Assert.Single(result);
        }
    }
}
