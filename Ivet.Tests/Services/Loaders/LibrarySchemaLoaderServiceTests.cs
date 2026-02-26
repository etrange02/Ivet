using Ivet.Services.Loaders;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Ivet.Tests.Services.Loaders
{
    public class LibrarySchemaLoaderServiceTests
    {
        [Fact]
        public void Load_DirectoryNotFound_ThrowsDirectoryNotFoundException()
        {
            // Arrange
            var service = new LibrarySchemaLoaderService(NullLogger<LibrarySchemaLoaderService>.Instance);
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => service.Load(path));
        }

        [Fact]
        public void Load_EmptyDirectory_ReturnsSchemaWithMigrationOnly()
        {
            // Arrange
            var service = new LibrarySchemaLoaderService(NullLogger<LibrarySchemaLoaderService>.Instance);
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);

            try
            {
                // Act
                var result = service.Load(path);

                // Assert
                Assert.NotNull(result);
                Assert.Single(result.Vertices);
                Assert.Empty(result.Edges);
            }
            finally
            {
                Directory.Delete(path);
            }
        }
    }
}
