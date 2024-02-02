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
            schemaSource.Vertices.Add(new MetaVertex { Name = _randomGenerator.RandomString() });
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
                Name = _randomGenerator.RandomString()              
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
            var metaVertex = new MetaVertex { Name = _randomGenerator.RandomString() };
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
            schemaSource.Edges.Add(new MetaEdge { Name = _randomGenerator.RandomString() });
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
                Name = _randomGenerator.RandomString()
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
            var metaEdge = new MetaEdge { Name = _randomGenerator.RandomString() };
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
            schemaSource.Properties.Add(new MetaPropertyKey { Name = _randomGenerator.RandomString() });
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
                Name = _randomGenerator.RandomString()
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
            var metaPropertyKey = new MetaPropertyKey { Name = _randomGenerator.RandomString() };
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
                Edge = _randomGenerator.RandomString(),
                Ingoing = _randomGenerator.RandomString(),
                Outgoing = _randomGenerator.RandomString()
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
                Edge = _randomGenerator.RandomString(),
                Ingoing = _randomGenerator.RandomString(),
                Outgoing = _randomGenerator.RandomString()
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
                Edge = _randomGenerator.RandomString(),
                Ingoing = _randomGenerator.RandomString(),
                Outgoing = _randomGenerator.RandomString()
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
                Entity = _randomGenerator.RandomString(),
                Name = _randomGenerator.RandomString(),
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
                Entity = _randomGenerator.RandomString(),
                Name = _randomGenerator.RandomString(),
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
                Entity = _randomGenerator.RandomString(),
                Name = _randomGenerator.RandomString(),
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
                Entity = _randomGenerator.RandomString(),
                Name = _randomGenerator.RandomString(),
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
                Entity = _randomGenerator.RandomString(),
                Name = _randomGenerator.RandomString(),
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
                Entity = _randomGenerator.RandomString(),
                Name = _randomGenerator.RandomString(),
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
                Name = _randomGenerator.RandomString(),
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
                Name = _randomGenerator.RandomString(),
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
                Name = _randomGenerator.RandomString(),
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
                Name = _randomGenerator.RandomString(),
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
                Name = _randomGenerator.RandomString(),
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
                Name = _randomGenerator.RandomString(),
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
                IndexName = _randomGenerator.RandomString(),
                PropertyName = _randomGenerator.RandomString(),
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
                IndexName = _randomGenerator.RandomString(),
                PropertyName = _randomGenerator.RandomString(),
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
                IndexName = _randomGenerator.RandomString(),
                PropertyName = _randomGenerator.RandomString(),
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
    }
}
