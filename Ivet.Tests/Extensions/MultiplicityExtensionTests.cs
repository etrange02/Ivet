using Ivet.Extensions;
using Ivet.Model;
using Xunit;

namespace Ivet.Tests.Extensions
{
    public class MultiplicityExtensionTests
    {
        [Theory]
        [InlineData(Multiplicity.MULTI, "MULTI")]
        [InlineData(Multiplicity.SIMPLE, "SIMPLE")]
        [InlineData(Multiplicity.MANY2ONE, "MANY2ONE")]
        [InlineData(Multiplicity.ONE2MANY, "ONE2MANY")]
        [InlineData(Multiplicity.ONE2ONE, "ONE2ONE")]
        public void ToJavaString_ValidMultiplicity_ReturnsExpected(Multiplicity multiplicity, string expected)
        {
            // Act
            var result = multiplicity.ToJavaString();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
