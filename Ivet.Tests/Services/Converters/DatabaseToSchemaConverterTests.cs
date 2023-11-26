using Ivet.Model;
using Ivet.Model.Database;
using Ivet.Services.Converters;
using Ivet.TestFramework;
using Xunit;

namespace Ivet.Tests.Services.Converters
{
    public class DatabaseToSchemaConverterTests
    {
        private RandomGenerator _random = new RandomGenerator();

        [Fact]
        public void ConvertTest_Empty()
        {
            // Arrange
            var schema = new Schema();

            // Act
            var result = DatabaseToSchemaConverter.Convert(schema);

            //Assert
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

        [Fact]
        public void ConvertTest_Vertex()
        {
            // Arrange
            var entity = new Vertex { Name = _random.RandomString(), Partitioned = _random.RandomBool(), Static = _random.RandomBool() };
            var schema = new Schema
            {
                Vertices = { entity }
            };

            // Act
            var result = DatabaseToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Single(result.Vertices);
            Assert.Equal(entity.Name, result.Vertices[0].Name);
            Assert.Equal(entity.Partitioned, result.Vertices[0].Partitioned);
            Assert.Equal(entity.Static, result.Vertices[0].Static);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Connections);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_Edge()
        {
            // Arrange
            var entity = new Edge { Name = _random.RandomString(), Directed = _random.RandomBool(), Multiplicity = _random.RandomEnum<Multiplicity>(), Unidirected = _random.RandomBool() };
            var schema = new Schema
            {
                Edges = { entity }
            };

            // Act
            var result = DatabaseToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Single(result.Edges);
            Assert.Equal(entity.Name, result.Edges[0].Name);
            Assert.Equal(entity.Multiplicity, result.Edges[0].Multiplicity);
            Assert.Empty(result.Connections);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_Connection()
        {
            // Arrange
            var entity = new Connection { Edge = _random.RandomString(), Ingoing = _random.RandomString(), Outgoing = _random.RandomString() };
            var schema = new Schema
            {
                Connections = { entity }
            };

            // Act
            var result = DatabaseToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Single(result.Connections);
            Assert.Equal(entity.Edge, result.Connections[0].Edge);
            Assert.Equal(entity.Ingoing, result.Connections[0].Ingoing);
            Assert.Equal(entity.Outgoing, result.Connections[0].Outgoing);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_PropertyKey()
        {
            // Arrange
            var entity = new PropertyKey { Name = _random.RandomString(), DataType = _random.RandomString(), Cardinality = _random.RandomEnum<Cardinality>() };
            var schema = new Schema
            {
                PropertyKeys = { entity }
            };

            // Act
            var result = DatabaseToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Connections);
            Assert.Single(result.Properties);
            Assert.Equal(entity.Name, result.Properties[0].Name);
            Assert.Equal(entity.DataType, result.Properties[0].DataType);
            Assert.Equal(entity.Cardinality, result.Properties[0].Cardinality);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_EdgePropertyKeyBinding()
        {
            // Arrange
            var entity = new PropertyBinding { Name = _random.RandomString(), Entity = _random.RandomString() };
            var schema = new Schema
            {
                EdgesPropertyBindings = { entity }
            };

            // Act
            var result = DatabaseToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Connections);
            Assert.Empty(result.Properties);
            Assert.Single(result.EdgePropertyBindings);
            Assert.Equal(entity.Name, result.EdgePropertyBindings[0].Name);
            Assert.Equal(entity.Entity, result.EdgePropertyBindings[0].Entity);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_VertexPropertyKeyBinding()
        {
            // Arrange
            var entity = new PropertyBinding { Name = _random.RandomString(), Entity = _random.RandomString() };
            var schema = new Schema
            {
                VertexPropertyBindings = { entity }
            };

            // Act
            var result = DatabaseToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Connections);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Single(result.VertexPropertyBindings);
            Assert.Equal(entity.Name, result.VertexPropertyBindings[0].Name);
            Assert.Equal(entity.Entity, result.VertexPropertyBindings[0].Entity);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_MixedIndices()
        {
            // Arrange
            var entity = new Model.Database.Index { Name = _random.RandomString(), BackendIndex = _random.RandomString(), IndexedElement = _random.RandomString(), IsUnique = _random.RandomBool(), IsCompositeIndex = false, IsMixedIndex = true };
            var schema = new Schema
            {
                Indices = { entity }
            };

            // Act
            var result = DatabaseToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Connections);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Single(result.MixedIndexes);
            Assert.Equal(entity.Name, result.MixedIndexes[0].Name);
            Assert.Equal(entity.BackendIndex, result.MixedIndexes[0].BackendIndex);
            Assert.Equal(entity.IndexedElement, result.MixedIndexes[0].IndexedElement);
            Assert.Empty(result.MixedIndexes[0].Kind);
            Assert.Empty(result.CompositeIndexes);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_CompositeIndices()
        {
            // Arrange
            var entity = new Model.Database.Index { Name = _random.RandomString(), BackendIndex = _random.RandomString(), IndexedElement = _random.RandomString(), IsUnique = _random.RandomBool(), IsCompositeIndex = true, IsMixedIndex = false };
            var schema = new Schema
            {
                Indices = { entity }
            };

            // Act
            var result = DatabaseToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Connections);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Single(result.CompositeIndexes);
            Assert.Equal(entity.Name, result.CompositeIndexes[0].Name);
            Assert.Equal(entity.IndexedElement, result.CompositeIndexes[0].IndexedElement);
            Assert.Equal(entity.IsUnique, result.CompositeIndexes[0].IsUnique);
            Assert.Empty(result.CompositeIndexes[0].Kind);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void ConvertTest_IndexBindings()
        {
            // Arrange
            var entity = new IndexBinding { IndexName = _random.RandomString(), Parameter = _random.RandomString(), PropertyName = _random.RandomString() };
            var schema = new Schema
            {
                IndexBindings = { entity }
            };

            // Act
            var result = DatabaseToSchemaConverter.Convert(schema);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.Connections);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EdgePropertyBindings);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.MixedIndexes);
            Assert.Empty(result.CompositeIndexes);
            Assert.Single(result.IndexBindings);
            Assert.Equal(entity.IndexName, result.IndexBindings[0].IndexName);
            Assert.Equal(entity.PropertyName, result.IndexBindings[0].PropertyName);
            Assert.Equal(MappingType.NULL, result.IndexBindings[0].Mapping);
        }
    }
}
