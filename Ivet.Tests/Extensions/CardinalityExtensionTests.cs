using Ivet.Extensions;
using Ivet.Model;
using Xunit;

namespace Ivet.Tests.Extensions
{
    public class CardinalityExtensionTests
    {
        [Theory]
        [InlineData(Cardinality.SINGLE, "org.janusgraph.core.Cardinality.SINGLE")]
        [InlineData(Cardinality.SET, "org.janusgraph.core.Cardinality.SET")]
        [InlineData(Cardinality.LIST, "org.janusgraph.core.Cardinality.LIST")]
        public void ToJavaString_ValidCardinality_ReturnsExpected(Cardinality cardinality, string expected)
        {
            // Act
            var result = cardinality.ToJavaString();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
