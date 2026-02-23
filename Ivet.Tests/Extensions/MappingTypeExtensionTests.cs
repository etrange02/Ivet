using Ivet.Extensions;
using Ivet.Model;
using Xunit;

namespace Ivet.Tests.Extensions
{
    public class MappingTypeExtensionTests
    {
        [Theory]
        [InlineData(MappingType.TEXT, "TEXT")]
        [InlineData(MappingType.TEXTSTRING, "TEXTSTRING")]
        [InlineData(MappingType.DEFAULT, "DEFAULT")]
        [InlineData(MappingType.STRING, "STRING")]
        [InlineData(MappingType.PREFIX_TREE, "PREFIX_TREE")]
        public void ToJavaString_ValidMapping_ReturnsExpected(MappingType type, string expected)
        {
            // Act
            var result = type.ToJavaString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToJavaString_NullMapping_Throws()
        {
            // Arrange
            var type = MappingType.NULL;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => type.ToJavaString());
        }
    }
}
