using Ivet.Model;
using Ivet.Services;
using Ivet.TestFramework;
using Xunit;

namespace Ivet.Tests.Services
{
    public class ParserTests
    {
        private RandomGenerator _randomGenerator = new RandomGenerator();

        [Fact]
        public void GetVerticesTest_Ok()
        {
            // Arrange
            var sut = new Parser();
            var label = RandomGenerator.RandomString();
            var isPartitionned = RandomGenerator.RandomBool();
            var isStatic = RandomGenerator.RandomBool();
            var content = $"Vertex Label Name  | Partitioned | Static  |{ Environment.NewLine }----{ Environment.NewLine }{label}    | {isPartitionned}       | {isStatic}   |";

            // Act
            var result = sut.GetVertices(content);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(label, result.First().Name);
            Assert.Equal(isPartitionned, result.First().Partitioned);
            Assert.Equal(isStatic, result.First().Static);
        }

        [Fact]
        public void GetEdgesTest_Ok()
        {
            // Arrange
            var sut = new Parser();
            var label = RandomGenerator.RandomString();
            var isDirected = RandomGenerator.RandomBool();
            var isUnidirected = RandomGenerator.RandomBool();
            var multiplicity = RandomGenerator.RandomEnum<Multiplicity>();
            var content = $"Edge Label Name  | Directed    | Unidirected | Multiplicity   |{ Environment.NewLine }------{ Environment.NewLine }{label}  | {isDirected} | {isUnidirected} | {multiplicity}  |";

            // Act
            var result = sut.GetEdges(content);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(label, result.First().Name);
            Assert.Equal(isDirected, result.First().Directed);
            Assert.Equal(isUnidirected, result.First().Unidirected);
            Assert.Equal(multiplicity, result.First().Multiplicity);
        }

        [Fact]
        public void GetPropertyKeysTest_Ok()
        {
            // Arrange
            var sut = new Parser();
            var label = RandomGenerator.RandomString();
            var cardinality = RandomGenerator.RandomEnum<Cardinality>();
            var dataType = RandomGenerator.RandomString();
            var content = $"Property Key Name | Cardinality | Data Type  |{ Environment.NewLine }---{ Environment.NewLine }{label}  | {cardinality}  | {dataType}  |";

            // Act
            var result = sut.GetPropertyKeys(content);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(label, result.First().Name);
            Assert.Equal(cardinality, result.First().Cardinality);
            Assert.Equal(dataType, result.First().DataType);
        }

        [Fact]
        public void GetConnectionsTest_Ok()
        {
            // Arrange
            var sut = new Parser();
            var edge = RandomGenerator.RandomString();
            var ingoing = RandomGenerator.RandomString();
            var outgoing = RandomGenerator.RandomString();
            var content = $"|Edge|Ingoing|Outgoing|\n|{edge}  | {ingoing}  | {outgoing}  |";

            // Act
            var result = sut.GetConnections(content);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(edge, result.First().Edge);
            Assert.Equal(ingoing, result.First().Ingoing);
            Assert.Equal(outgoing, result.First().Outgoing);
        }

        [Fact]
        public void GetPropertyBindingsTest_Ok()
        {
            // Arrange
            var sut = new Parser();
            var name = RandomGenerator.RandomString();
            var entity = RandomGenerator.RandomString();
            var content = $"|Name|Entity|\n|{name}  | {entity}  |";

            // Act
            var result = sut.GetPropertyBindings(content);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(name, result.First().Name);
            Assert.Equal(entity, result.First().Entity);
        }

        [Fact]
        public void GetIndicesTest_Ok()
        {
            // Arrange
            var sut = new Parser();
            var name = RandomGenerator.RandomString();
            var isUnique = RandomGenerator.RandomBool();
            var isMixedIndex = RandomGenerator.RandomBool();
            var isCompositeIndex = RandomGenerator.RandomBool();
            var backendIndex = RandomGenerator.RandomString();
            var indexedElement = RandomGenerator.RandomString();
            var content = $"|Name|IsUnique|IsMixedIndex|IsCompositeIndex|BackendIndex|IndexedElement|\n|{name}|{isUnique}|{isMixedIndex}|{isCompositeIndex}|{backendIndex}|{indexedElement}|";

            // Act
            var result = sut.GetIndices(content);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(name, result.First().Name);
            Assert.Equal(isUnique, result.First().IsUnique);
            Assert.Equal(isMixedIndex, result.First().IsMixedIndex);
            Assert.Equal(isCompositeIndex, result.First().IsCompositeIndex);
            Assert.Equal(backendIndex, result.First().BackendIndex);
            Assert.Equal(indexedElement, result.First().IndexedElement);
        }

        [Fact]
        public void GetIndexBindingsTest_Ok()
        {
            // Arrange
            var sut = new Parser();
            var indexName = RandomGenerator.RandomString();
            var propertyName = RandomGenerator.RandomString();
            var parameter = RandomGenerator.RandomString();
            var content = $"|IndexName|PropertyName|Parameter|\n|{indexName}|{propertyName}|{parameter}|";

            // Act
            var result = sut.GetIndexBindings(content);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(indexName, result.First().IndexName);
            Assert.Equal(propertyName, result.First().PropertyName);
            Assert.Equal(parameter, result.First().Parameter);
        }
    }
}
