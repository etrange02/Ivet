using Ivet.Services;
using Ivet.Services.Loaders;
using Moq;
using Xunit;

namespace Ivet.Tests.Services.Loaders
{
    public class DatabaseSchemaLoaderServiceTests
    {
        [Fact]
        public void Load_Vertex()
        {
            // Arrange
            var databaseServiceMock = new Mock<IDatabaseService>();
            databaseServiceMock.Setup(x => x.GetVertexSchema()).Returns("Vertex Label Name | Partitioned | Static  |\r\n----\r\n Label | true | false |");
            databaseServiceMock.Setup(x => x.GetEdgeSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetPropertyKeysSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetConnectionSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetVertexPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgesPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexBindingSchema()).Returns("");
            var sut = new DatabaseSchemaLoaderService(databaseServiceMock.Object);

            // Act
            var result = sut.Load();

            // Assert
            Assert.Single(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.PropertyKeys);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgesPropertyBindings);
            Assert.Empty(result.Indices);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void Load_Edge()
        {
            // Arrange
            var databaseServiceMock = new Mock<IDatabaseService>();
            databaseServiceMock.Setup(x => x.GetVertexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgeSchema()).Returns("Edge Label Name | Directed| Unidirected | Multiplicity   |\r\n------\r\n{label}  | true | true | SIMPLE |");
            databaseServiceMock.Setup(x => x.GetPropertyKeysSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetConnectionSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetVertexPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgesPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexBindingSchema()).Returns("");
            var sut = new DatabaseSchemaLoaderService(databaseServiceMock.Object);

            // Act
            var result = sut.Load();

            // Assert
            Assert.Empty(result.Vertices);
            Assert.Single(result.Edges);
            Assert.Empty(result.PropertyKeys);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgesPropertyBindings);
            Assert.Empty(result.Indices);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void Load_PropertyKey()
        {
            // Arrange
            var databaseServiceMock = new Mock<IDatabaseService>();
            databaseServiceMock.Setup(x => x.GetVertexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgeSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetPropertyKeysSchema()).Returns("Property Key Name | Cardinality | Data Type  |\r\n---\r\n{label} | SINGLE  | {dataType}  |");
            databaseServiceMock.Setup(x => x.GetConnectionSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetVertexPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgesPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexBindingSchema()).Returns("");
            var sut = new DatabaseSchemaLoaderService(databaseServiceMock.Object);

            // Act
            var result = sut.Load();

            // Assert
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Single(result.PropertyKeys);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgesPropertyBindings);
            Assert.Empty(result.Indices);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void Load_Connection()
        {
            // Arrange
            var databaseServiceMock = new Mock<IDatabaseService>();
            databaseServiceMock.Setup(x => x.GetVertexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgeSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetPropertyKeysSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetConnectionSchema()).Returns("|Edge|Ingoing|Outgoing|\n|{edge}  | {ingoing}  | {outgoing}  |");
            databaseServiceMock.Setup(x => x.GetVertexPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgesPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexBindingSchema()).Returns("");
            var sut = new DatabaseSchemaLoaderService(databaseServiceMock.Object);

            // Act
            var result = sut.Load();

            // Assert
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.PropertyKeys);
            Assert.Single(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgesPropertyBindings);
            Assert.Empty(result.Indices);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void Load_VertexPropertyBinding()
        {
            // Arrange
            var databaseServiceMock = new Mock<IDatabaseService>();
            databaseServiceMock.Setup(x => x.GetVertexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgeSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetPropertyKeysSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetConnectionSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetVertexPropertyBindingsSchema()).Returns("|Name|Entity|\n|{name}  | {entity}  |");
            databaseServiceMock.Setup(x => x.GetEdgesPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexBindingSchema()).Returns("");
            var sut = new DatabaseSchemaLoaderService(databaseServiceMock.Object);

            // Act
            var result = sut.Load();

            // Assert
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.PropertyKeys);
            Assert.Empty(result.Connections);
            Assert.Single(result.VertexPropertyBindings);
            Assert.Empty(result.EdgesPropertyBindings);
            Assert.Empty(result.Indices);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void Load_EdgePropertyBinding()
        {
            // Arrange
            var databaseServiceMock = new Mock<IDatabaseService>();
            databaseServiceMock.Setup(x => x.GetVertexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgeSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetPropertyKeysSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetConnectionSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetVertexPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgesPropertyBindingsSchema()).Returns("|Name|Entity|\n|{name}  | {entity}  |");
            databaseServiceMock.Setup(x => x.GetIndexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexBindingSchema()).Returns("");
            var sut = new DatabaseSchemaLoaderService(databaseServiceMock.Object);

            // Act
            var result = sut.Load();

            // Assert
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.PropertyKeys);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Single(result.EdgesPropertyBindings);
            Assert.Empty(result.Indices);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void Load_Index()
        {
            // Arrange
            var databaseServiceMock = new Mock<IDatabaseService>();
            databaseServiceMock.Setup(x => x.GetVertexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgeSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetPropertyKeysSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetConnectionSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetVertexPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgesPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexSchema()).Returns("|Name|IsUnique|IsMixedIndex|IsCompositeIndex|BackendIndex|IndexedElement|\n|{name}|true|true|true|{backendIndex}|{indexedElement}|");
            databaseServiceMock.Setup(x => x.GetIndexBindingSchema()).Returns("");
            var sut = new DatabaseSchemaLoaderService(databaseServiceMock.Object);

            // Act
            var result = sut.Load();

            // Assert
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.PropertyKeys);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgesPropertyBindings);
            Assert.Single(result.Indices);
            Assert.Empty(result.IndexBindings);
        }

        [Fact]
        public void Load_IndexBinding()
        {
            // Arrange
            var databaseServiceMock = new Mock<IDatabaseService>();
            databaseServiceMock.Setup(x => x.GetVertexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgeSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetPropertyKeysSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetConnectionSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetVertexPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetEdgesPropertyBindingsSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexSchema()).Returns("");
            databaseServiceMock.Setup(x => x.GetIndexBindingSchema()).Returns("|IndexName|PropertyName|Parameter|\n|{indexName}|{propertyName}|{parameter}|");
            var sut = new DatabaseSchemaLoaderService(databaseServiceMock.Object);

            // Act
            var result = sut.Load();

            // Assert
            Assert.Empty(result.Vertices);
            Assert.Empty(result.Edges);
            Assert.Empty(result.PropertyKeys);
            Assert.Empty(result.Connections);
            Assert.Empty(result.VertexPropertyBindings);
            Assert.Empty(result.EdgesPropertyBindings);
            Assert.Empty(result.Indices);
            Assert.Single(result.IndexBindings);
        }
    }
}
