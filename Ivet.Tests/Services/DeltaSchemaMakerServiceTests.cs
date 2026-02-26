using Ivet.Model;
using Ivet.Model.Meta;
using Ivet.Services;
using Ivet.TestFramework;
using Xunit;

namespace Ivet.Tests.Services
{
    public class DeltaSchemaMakerServiceTests
    {
        private RandomGenerator _randomGenerator = new RandomGenerator();

        [Fact]
        public void DifferenceTest_VertexInSourceSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            schemaSource.Vertices.Add(new MetaVertex { Name = RandomGenerator.RandomString() });
            var schemaTarget = new MetaSchema();

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_VertexInTargetSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            schemaTarget.Vertices.Add(new MetaVertex { 
                Name = RandomGenerator.RandomString()              
            });

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Single(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
            Assert.Equal(schemaTarget.Vertices[0], result.Vertices[0]);
        }

        [Fact]
        public void DifferenceTest_VertexInBothSourceAndTargetSchema()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            var metaVertex = new MetaVertex { Name = RandomGenerator.RandomString() };
            schemaTarget.Vertices.Add(metaVertex);
            schemaSource.Vertices.Add(metaVertex);

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }


        [Fact]
        public void DifferenceTest_EdgeInSourceSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            schemaSource.Edges.Add(new MetaEdge { Name = RandomGenerator.RandomString() });
            var schemaTarget = new MetaSchema();

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_EdgeInTargetSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            schemaTarget.Edges.Add(new MetaEdge
            {
                Name = RandomGenerator.RandomString()
            });

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Single(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
            Assert.Equal(schemaTarget.Edges[0], result.Edges[0]);
        }

        [Fact]
        public void DifferenceTest_EdgeInBothSourceAndTargetSchema()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            var metaEdge = new MetaEdge { Name = RandomGenerator.RandomString() };
            schemaTarget.Edges.Add(metaEdge);
            schemaSource.Edges.Add(metaEdge);

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_PropertyInSourceSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            schemaSource.Properties.Add(new MetaPropertyKey { Name = RandomGenerator.RandomString() });
            var schemaTarget = new MetaSchema();

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_PropertyInTargetSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            schemaTarget.Properties.Add(new MetaPropertyKey
            {
                Name = RandomGenerator.RandomString()
            });

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Single(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
            Assert.Equal(schemaTarget.Properties[0], result.Properties[0]);
        }

        [Fact]
        public void DifferenceTest_PropertyInBothSourceAndTargetSchema()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            var metaPropertyKey = new MetaPropertyKey { Name = RandomGenerator.RandomString() };
            schemaTarget.Properties.Add(metaPropertyKey);
            schemaSource.Properties.Add(metaPropertyKey);

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_ConnectionInSourceSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            schemaSource.Connections.Add(new MetaConnection { 
                Edge = RandomGenerator.RandomString(),
                Ingoing = RandomGenerator.RandomString(),
                Outgoing = RandomGenerator.RandomString()
            });
            var schemaTarget = new MetaSchema();

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_ConnectionInTargetSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            schemaTarget.Connections.Add(new MetaConnection
            {
                Edge = RandomGenerator.RandomString(),
                Ingoing = RandomGenerator.RandomString(),
                Outgoing = RandomGenerator.RandomString()
            });

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Single(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
            Assert.Equal(schemaTarget.Connections[0], result.Connections[0]);
        }

        [Fact]
        public void DifferenceTest_ConnectionInBothSourceAndTargetSchema()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            var metaConnection = new MetaConnection
            {
                Edge = RandomGenerator.RandomString(),
                Ingoing = RandomGenerator.RandomString(),
                Outgoing = RandomGenerator.RandomString()
            };
            schemaTarget.Connections.Add(metaConnection);
            schemaSource.Connections.Add(metaConnection);

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_VertexPropertyBindingInSourceSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            schemaSource.VertexPropertyBindings.Add(new MetaPropertyBinding
            {
                Entity = RandomGenerator.RandomString(),
                Name = RandomGenerator.RandomString(),
            });
            var schemaTarget = new MetaSchema();

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_VertexPropertyBindingInTargetSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            schemaTarget.VertexPropertyBindings.Add(new MetaPropertyBinding
            {
                Entity = RandomGenerator.RandomString(),
                Name = RandomGenerator.RandomString(),
            });

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Single(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
            Assert.Equal(schemaTarget.VertexPropertyBindings[0], result.VertexPropertyBindings[0]);
        }

        [Fact]
        public void DifferenceTest_VertexPropertyBindingInBothSourceAndTargetSchema()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            var metaPropertyBinding = new MetaPropertyBinding
            {
                Entity = RandomGenerator.RandomString(),
                Name = RandomGenerator.RandomString(),
            };
            schemaTarget.VertexPropertyBindings.Add(metaPropertyBinding);
            schemaSource.VertexPropertyBindings.Add(metaPropertyBinding);

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_EdgePropertyBindingInSourceSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            schemaSource.EdgePropertyBindings.Add(new MetaPropertyBinding
            {
                Entity = RandomGenerator.RandomString(),
                Name = RandomGenerator.RandomString(),
            });
            var schemaTarget = new MetaSchema();

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_EdgePropertyBindingInTargetSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            schemaTarget.EdgePropertyBindings.Add(new MetaPropertyBinding
            {
                Entity = RandomGenerator.RandomString(),
                Name = RandomGenerator.RandomString(),
            });

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Single(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
            Assert.Equal(schemaTarget.EdgePropertyBindings[0], result.EdgePropertyBindings[0]);
        }

        [Fact]
        public void DifferenceTest_EdgePropertyBindingInBothSourceAndTargetSchema()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            var metaPropertyBinding = new MetaPropertyBinding
            {
                Entity = RandomGenerator.RandomString(),
                Name = RandomGenerator.RandomString(),
            };
            schemaTarget.EdgePropertyBindings.Add(metaPropertyBinding);
            schemaSource.EdgePropertyBindings.Add(metaPropertyBinding);

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_CompositeIndexInSourceSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            schemaSource.CompositeIndexes.Add(new MetaCompositeIndex
            {
                Name = RandomGenerator.RandomString(),
            });
            var schemaTarget = new MetaSchema();

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_CompositeIndexInTargetSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            schemaTarget.CompositeIndexes.Add(new MetaCompositeIndex
            {
                Name = RandomGenerator.RandomString(),
            });

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Single(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
            Assert.Equal(schemaTarget.CompositeIndexes[0], result.CompositeIndexes[0]);
        }

        [Fact]
        public void DifferenceTest_CompositeIndexInBothSourceAndTargetSchema()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            var metaCompositeIndex = new MetaCompositeIndex
            {
                Name = RandomGenerator.RandomString(),
            };
            schemaTarget.CompositeIndexes.Add(metaCompositeIndex);
            schemaSource.CompositeIndexes.Add(metaCompositeIndex);

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }
        
        [Fact]
        public void DifferenceTest_MixedIndexInSourceSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            schemaSource.MixedIndexes.Add(new MetaMixedIndex
            {
                Name = RandomGenerator.RandomString(),
            });
            var schemaTarget = new MetaSchema();

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_MixedIndexInTargetSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            schemaTarget.MixedIndexes.Add(new MetaMixedIndex
            {
                Name = RandomGenerator.RandomString(),
            });

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Single(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
            Assert.Equal(schemaTarget.MixedIndexes[0], result.MixedIndexes[0]);
        }

        [Fact]
        public void DifferenceTest_MixedIndexInBothSourceAndTargetSchema()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            var metaMixedIndex = new MetaMixedIndex
            {
                Name = RandomGenerator.RandomString(),
            };
            schemaTarget.MixedIndexes.Add(metaMixedIndex);
            schemaSource.MixedIndexes.Add(metaMixedIndex);

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_IndexBindingInSourceSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            schemaSource.IndexBindings.Add(new MetaIndexBinding
            {
                IndexName = RandomGenerator.RandomString(),
                PropertyName = RandomGenerator.RandomString(),
            });
            var schemaTarget = new MetaSchema();

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void DifferenceTest_IndexBindingInTargetSchemaOnly()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            schemaTarget.IndexBindings.Add(new MetaIndexBinding
            {
                IndexName = RandomGenerator.RandomString(),
                PropertyName = RandomGenerator.RandomString(),
            });

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Single(result.IndexBindings);
            Assert.Equal(schemaTarget.IndexBindings[0], result.IndexBindings[0]);
        }

        [Fact]
        public void DifferenceTest_IndexBindingInBothSourceAndTargetSchema()
        {
            // Arrange
            var sut = new DeltaSchemaMakerService();
            var schemaSource = new MetaSchema();
            var schemaTarget = new MetaSchema();
            var metaIndexBinding = new MetaIndexBinding
            {
                IndexName = RandomGenerator.RandomString(),
                PropertyName = RandomGenerator.RandomString(),
            };
            schemaTarget.IndexBindings.Add(metaIndexBinding);
            schemaSource.IndexBindings.Add(metaIndexBinding);

            // Act
            var result = sut.Difference(schemaSource, schemaTarget);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Properties);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.IndexBindings);
        }

        // ==================== Removals Tests ====================

        [Fact]
        public void Removals_VertexInSourceOnly_ReturnsVertex()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            source.Vertices.Add(new MetaVertex { Name = RandomGenerator.RandomString() });
            var target = new MetaSchema();

            var result = sut.Removals(source, target);

            Assert.Single(result.Vertices);
            Assert.Equal(source.Vertices[0], result.Vertices[0]);
        }

        [Fact]
        public void Removals_VertexInTargetOnly_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            var target = new MetaSchema();
            target.Vertices.Add(new MetaVertex { Name = RandomGenerator.RandomString() });

            var result = sut.Removals(source, target);

            Assert.Empty(result.Vertices);
        }

        [Fact]
        public void Removals_VertexInBoth_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var vertex = new MetaVertex { Name = RandomGenerator.RandomString() };
            var source = new MetaSchema();
            source.Vertices.Add(vertex);
            var target = new MetaSchema();
            target.Vertices.Add(vertex);

            var result = sut.Removals(source, target);

            Assert.Empty(result.Vertices);
        }

        [Fact]
        public void Removals_EdgeInSourceOnly_ReturnsEdge()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            source.Edges.Add(new MetaEdge { Name = RandomGenerator.RandomString() });
            var target = new MetaSchema();

            var result = sut.Removals(source, target);

            Assert.Single(result.Edges);
            Assert.Equal(source.Edges[0], result.Edges[0]);
        }

        [Fact]
        public void Removals_EdgeInTargetOnly_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            var target = new MetaSchema();
            target.Edges.Add(new MetaEdge { Name = RandomGenerator.RandomString() });

            var result = sut.Removals(source, target);

            Assert.Empty(result.Edges);
        }

        [Fact]
        public void Removals_EdgeInBoth_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var edge = new MetaEdge { Name = RandomGenerator.RandomString() };
            var source = new MetaSchema();
            source.Edges.Add(edge);
            var target = new MetaSchema();
            target.Edges.Add(edge);

            var result = sut.Removals(source, target);

            Assert.Empty(result.Edges);
        }

        [Fact]
        public void Removals_PropertyInSourceOnly_ReturnsProperty()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            source.Properties.Add(new MetaPropertyKey { Name = RandomGenerator.RandomString() });
            var target = new MetaSchema();

            var result = sut.Removals(source, target);

            Assert.Single(result.Properties);
            Assert.Equal(source.Properties[0], result.Properties[0]);
        }

        [Fact]
        public void Removals_PropertyInTargetOnly_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            var target = new MetaSchema();
            target.Properties.Add(new MetaPropertyKey { Name = RandomGenerator.RandomString() });

            var result = sut.Removals(source, target);

            Assert.Empty(result.Properties);
        }

        [Fact]
        public void Removals_PropertyInBoth_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var prop = new MetaPropertyKey { Name = RandomGenerator.RandomString() };
            var source = new MetaSchema();
            source.Properties.Add(prop);
            var target = new MetaSchema();
            target.Properties.Add(prop);

            var result = sut.Removals(source, target);

            Assert.Empty(result.Properties);
        }

        [Fact]
        public void Removals_ConnectionInSourceOnly_ReturnsConnection()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            source.Connections.Add(new MetaConnection
            {
                Edge = RandomGenerator.RandomString(),
                Ingoing = RandomGenerator.RandomString(),
                Outgoing = RandomGenerator.RandomString()
            });
            var target = new MetaSchema();

            var result = sut.Removals(source, target);

            Assert.Single(result.Connections);
        }

        [Fact]
        public void Removals_ConnectionInTargetOnly_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            var target = new MetaSchema();
            target.Connections.Add(new MetaConnection
            {
                Edge = RandomGenerator.RandomString(),
                Ingoing = RandomGenerator.RandomString(),
                Outgoing = RandomGenerator.RandomString()
            });

            var result = sut.Removals(source, target);

            Assert.Empty(result.Connections);
        }

        [Fact]
        public void Removals_ConnectionInBoth_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var conn = new MetaConnection
            {
                Edge = RandomGenerator.RandomString(),
                Ingoing = RandomGenerator.RandomString(),
                Outgoing = RandomGenerator.RandomString()
            };
            var source = new MetaSchema();
            source.Connections.Add(conn);
            var target = new MetaSchema();
            target.Connections.Add(conn);

            var result = sut.Removals(source, target);

            Assert.Empty(result.Connections);
        }

        [Fact]
        public void Removals_VertexPropertyBindingInSourceOnly_ReturnsBinding()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            source.VertexPropertyBindings.Add(new MetaPropertyBinding
            {
                Name = RandomGenerator.RandomString(),
                Entity = RandomGenerator.RandomString()
            });
            var target = new MetaSchema();

            var result = sut.Removals(source, target);

            Assert.Single(result.VertexPropertyBindings);
        }

        [Fact]
        public void Removals_VertexPropertyBindingInTargetOnly_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            var target = new MetaSchema();
            target.VertexPropertyBindings.Add(new MetaPropertyBinding
            {
                Name = RandomGenerator.RandomString(),
                Entity = RandomGenerator.RandomString()
            });

            var result = sut.Removals(source, target);

            Assert.Empty(result.VertexPropertyBindings);
        }

        [Fact]
        public void Removals_VertexPropertyBindingInBoth_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var binding = new MetaPropertyBinding
            {
                Name = RandomGenerator.RandomString(),
                Entity = RandomGenerator.RandomString()
            };
            var source = new MetaSchema();
            source.VertexPropertyBindings.Add(binding);
            var target = new MetaSchema();
            target.VertexPropertyBindings.Add(binding);

            var result = sut.Removals(source, target);

            Assert.Empty(result.VertexPropertyBindings);
        }

        [Fact]
        public void Removals_EdgePropertyBindingInSourceOnly_ReturnsBinding()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            source.EdgePropertyBindings.Add(new MetaPropertyBinding
            {
                Name = RandomGenerator.RandomString(),
                Entity = RandomGenerator.RandomString()
            });
            var target = new MetaSchema();

            var result = sut.Removals(source, target);

            Assert.Single(result.EdgePropertyBindings);
        }

        [Fact]
        public void Removals_EdgePropertyBindingInTargetOnly_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            var target = new MetaSchema();
            target.EdgePropertyBindings.Add(new MetaPropertyBinding
            {
                Name = RandomGenerator.RandomString(),
                Entity = RandomGenerator.RandomString()
            });

            var result = sut.Removals(source, target);

            Assert.Empty(result.EdgePropertyBindings);
        }

        [Fact]
        public void Removals_EdgePropertyBindingInBoth_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var binding = new MetaPropertyBinding
            {
                Name = RandomGenerator.RandomString(),
                Entity = RandomGenerator.RandomString()
            };
            var source = new MetaSchema();
            source.EdgePropertyBindings.Add(binding);
            var target = new MetaSchema();
            target.EdgePropertyBindings.Add(binding);

            var result = sut.Removals(source, target);

            Assert.Empty(result.EdgePropertyBindings);
        }

        [Fact]
        public void Removals_CompositeIndexInSourceOnly_ReturnsIndex()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            source.CompositeIndexes.Add(new MetaCompositeIndex { Name = RandomGenerator.RandomString() });
            var target = new MetaSchema();

            var result = sut.Removals(source, target);

            Assert.Single(result.CompositeIndexes);
        }

        [Fact]
        public void Removals_CompositeIndexInTargetOnly_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            var target = new MetaSchema();
            target.CompositeIndexes.Add(new MetaCompositeIndex { Name = RandomGenerator.RandomString() });

            var result = sut.Removals(source, target);

            Assert.Empty(result.CompositeIndexes);
        }

        [Fact]
        public void Removals_CompositeIndexInBoth_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var index = new MetaCompositeIndex { Name = RandomGenerator.RandomString() };
            var source = new MetaSchema();
            source.CompositeIndexes.Add(index);
            var target = new MetaSchema();
            target.CompositeIndexes.Add(index);

            var result = sut.Removals(source, target);

            Assert.Empty(result.CompositeIndexes);
        }

        [Fact]
        public void Removals_MixedIndexInSourceOnly_ReturnsIndex()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            source.MixedIndexes.Add(new MetaMixedIndex { Name = RandomGenerator.RandomString() });
            var target = new MetaSchema();

            var result = sut.Removals(source, target);

            Assert.Single(result.MixedIndexes);
        }

        [Fact]
        public void Removals_MixedIndexInTargetOnly_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            var target = new MetaSchema();
            target.MixedIndexes.Add(new MetaMixedIndex { Name = RandomGenerator.RandomString() });

            var result = sut.Removals(source, target);

            Assert.Empty(result.MixedIndexes);
        }

        [Fact]
        public void Removals_MixedIndexInBoth_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var index = new MetaMixedIndex { Name = RandomGenerator.RandomString() };
            var source = new MetaSchema();
            source.MixedIndexes.Add(index);
            var target = new MetaSchema();
            target.MixedIndexes.Add(index);

            var result = sut.Removals(source, target);

            Assert.Empty(result.MixedIndexes);
        }

        [Fact]
        public void Removals_IndexBindingInSourceOnly_ReturnsBinding()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            source.IndexBindings.Add(new MetaIndexBinding
            {
                IndexName = RandomGenerator.RandomString(),
                PropertyName = RandomGenerator.RandomString()
            });
            var target = new MetaSchema();

            var result = sut.Removals(source, target);

            Assert.Single(result.IndexBindings);
        }

        [Fact]
        public void Removals_IndexBindingInTargetOnly_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            var target = new MetaSchema();
            target.IndexBindings.Add(new MetaIndexBinding
            {
                IndexName = RandomGenerator.RandomString(),
                PropertyName = RandomGenerator.RandomString()
            });

            var result = sut.Removals(source, target);

            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void Removals_IndexBindingInBoth_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var binding = new MetaIndexBinding
            {
                IndexName = RandomGenerator.RandomString(),
                PropertyName = RandomGenerator.RandomString()
            };
            var source = new MetaSchema();
            source.IndexBindings.Add(binding);
            var target = new MetaSchema();
            target.IndexBindings.Add(binding);

            var result = sut.Removals(source, target);

            Assert.Empty(result.IndexBindings);
        }

        // ==================== Modifications Tests ====================

        [Fact]
        public void Modifications_EmptySchemas_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var result = sut.Modifications(new MetaSchema(), new MetaSchema());
            Assert.Empty(result);
        }

        [Fact]
        public void Modifications_VertexOnlyInSource_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            source.Vertices.Add(new MetaVertex { Name = "V1", Partitioned = true });
            var target = new MetaSchema();

            var result = sut.Modifications(source, target);

            Assert.Empty(result);
        }

        [Fact]
        public void Modifications_VertexOnlyInTarget_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var source = new MetaSchema();
            var target = new MetaSchema();
            target.Vertices.Add(new MetaVertex { Name = "V1", Partitioned = true });

            var result = sut.Modifications(source, target);

            Assert.Empty(result);
        }

        [Fact]
        public void Modifications_VertexPartitionedChanged_ReturnsModification()
        {
            var sut = new DeltaSchemaMakerService();
            var name = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.Vertices.Add(new MetaVertex { Name = name, Partitioned = false });
            var target = new MetaSchema();
            target.Vertices.Add(new MetaVertex { Name = name, Partitioned = true });

            var result = sut.Modifications(source, target);

            Assert.Single(result);
            Assert.Equal("Vertex", result[0].ElementType);
            Assert.Equal(name, result[0].ElementName);
            Assert.Equal("Partitioned", result[0].PropertyName);
        }

        [Fact]
        public void Modifications_VertexStaticChanged_ReturnsModification()
        {
            var sut = new DeltaSchemaMakerService();
            var name = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.Vertices.Add(new MetaVertex { Name = name, Static = false });
            var target = new MetaSchema();
            target.Vertices.Add(new MetaVertex { Name = name, Static = true });

            var result = sut.Modifications(source, target);

            Assert.Single(result);
            Assert.Equal("Static", result[0].PropertyName);
        }

        [Fact]
        public void Modifications_VertexNoChanges_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var name = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.Vertices.Add(new MetaVertex { Name = name, Partitioned = true, Static = false });
            var target = new MetaSchema();
            target.Vertices.Add(new MetaVertex { Name = name, Partitioned = true, Static = false });

            var result = sut.Modifications(source, target);

            Assert.Empty(result);
        }

        [Fact]
        public void Modifications_EdgeMultiplicityChanged_ReturnsModification()
        {
            var sut = new DeltaSchemaMakerService();
            var name = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.Edges.Add(new MetaEdge { Name = name, Multiplicity = Multiplicity.MULTI });
            var target = new MetaSchema();
            target.Edges.Add(new MetaEdge { Name = name, Multiplicity = Multiplicity.SIMPLE });

            var result = sut.Modifications(source, target);

            Assert.Single(result);
            Assert.Equal("Edge", result[0].ElementType);
            Assert.Equal("Multiplicity", result[0].PropertyName);
        }

        [Fact]
        public void Modifications_EdgeNoChanges_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var name = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.Edges.Add(new MetaEdge { Name = name, Multiplicity = Multiplicity.MULTI });
            var target = new MetaSchema();
            target.Edges.Add(new MetaEdge { Name = name, Multiplicity = Multiplicity.MULTI });

            var result = sut.Modifications(source, target);

            Assert.Empty(result);
        }

        [Fact]
        public void Modifications_PropertyCardinalityChanged_ReturnsModification()
        {
            var sut = new DeltaSchemaMakerService();
            var name = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.Properties.Add(new MetaPropertyKey { Name = name, Cardinality = Cardinality.SINGLE });
            var target = new MetaSchema();
            target.Properties.Add(new MetaPropertyKey { Name = name, Cardinality = Cardinality.LIST });

            var result = sut.Modifications(source, target);

            Assert.Single(result);
            Assert.Equal("PropertyKey", result[0].ElementType);
            Assert.Equal("Cardinality", result[0].PropertyName);
        }

        [Fact]
        public void Modifications_PropertyDataTypeChanged_ReturnsModification()
        {
            var sut = new DeltaSchemaMakerService();
            var name = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.Properties.Add(new MetaPropertyKey { Name = name, DataType = "String.class" });
            var target = new MetaSchema();
            target.Properties.Add(new MetaPropertyKey { Name = name, DataType = "Integer.class" });

            var result = sut.Modifications(source, target);

            Assert.Single(result);
            Assert.Equal("DataType", result[0].PropertyName);
        }

        [Fact]
        public void Modifications_PropertyNoChanges_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var name = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.Properties.Add(new MetaPropertyKey { Name = name, Cardinality = Cardinality.SINGLE, DataType = "String.class" });
            var target = new MetaSchema();
            target.Properties.Add(new MetaPropertyKey { Name = name, Cardinality = Cardinality.SINGLE, DataType = "String.class" });

            var result = sut.Modifications(source, target);

            Assert.Empty(result);
        }

        [Fact]
        public void Modifications_CompositeIndexIsUniqueChanged_ReturnsModification()
        {
            var sut = new DeltaSchemaMakerService();
            var name = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.CompositeIndexes.Add(new MetaCompositeIndex { Name = name, IsUnique = false });
            var target = new MetaSchema();
            target.CompositeIndexes.Add(new MetaCompositeIndex { Name = name, IsUnique = true });

            var result = sut.Modifications(source, target);

            Assert.Single(result);
            Assert.Equal("CompositeIndex", result[0].ElementType);
            Assert.Equal("IsUnique", result[0].PropertyName);
        }

        [Fact]
        public void Modifications_CompositeIndexNoChanges_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var name = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.CompositeIndexes.Add(new MetaCompositeIndex { Name = name, IsUnique = true });
            var target = new MetaSchema();
            target.CompositeIndexes.Add(new MetaCompositeIndex { Name = name, IsUnique = true });

            var result = sut.Modifications(source, target);

            Assert.Empty(result);
        }

        [Fact]
        public void Modifications_MixedIndexBackendChanged_ReturnsModification()
        {
            var sut = new DeltaSchemaMakerService();
            var name = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.MixedIndexes.Add(new MetaMixedIndex { Name = name, BackendIndex = "search" });
            var target = new MetaSchema();
            target.MixedIndexes.Add(new MetaMixedIndex { Name = name, BackendIndex = "solr" });

            var result = sut.Modifications(source, target);

            Assert.Single(result);
            Assert.Equal("MixedIndex", result[0].ElementType);
            Assert.Equal("BackendIndex", result[0].PropertyName);
        }

        [Fact]
        public void Modifications_MixedIndexNoChanges_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var name = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.MixedIndexes.Add(new MetaMixedIndex { Name = name, BackendIndex = "search" });
            var target = new MetaSchema();
            target.MixedIndexes.Add(new MetaMixedIndex { Name = name, BackendIndex = "search" });

            var result = sut.Modifications(source, target);

            Assert.Empty(result);
        }

        [Fact]
        public void Modifications_IndexBindingMappingChanged_ReturnsModification()
        {
            var sut = new DeltaSchemaMakerService();
            var indexName = RandomGenerator.RandomString();
            var propName = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.IndexBindings.Add(new MetaIndexBinding { IndexName = indexName, PropertyName = propName, Mapping = MappingType.TEXT });
            var target = new MetaSchema();
            target.IndexBindings.Add(new MetaIndexBinding { IndexName = indexName, PropertyName = propName, Mapping = MappingType.STRING });

            var result = sut.Modifications(source, target);

            Assert.Single(result);
            Assert.Equal("IndexBinding", result[0].ElementType);
            Assert.Equal("Mapping", result[0].PropertyName);
        }

        [Fact]
        public void Modifications_IndexBindingNoChanges_ReturnsEmpty()
        {
            var sut = new DeltaSchemaMakerService();
            var indexName = RandomGenerator.RandomString();
            var propName = RandomGenerator.RandomString();
            var source = new MetaSchema();
            source.IndexBindings.Add(new MetaIndexBinding { IndexName = indexName, PropertyName = propName, Mapping = MappingType.TEXT });
            var target = new MetaSchema();
            target.IndexBindings.Add(new MetaIndexBinding { IndexName = indexName, PropertyName = propName, Mapping = MappingType.TEXT });

            var result = sut.Modifications(source, target);

            Assert.Empty(result);
        }
    }
}
