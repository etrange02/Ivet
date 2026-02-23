using Ivet.Services;
using Xunit;

namespace Ivet.Tests.Services
{
    public class GremlinIdentifierValidatorTests
    {
        [Theory]
        [InlineData("MyVertex", "vertex name")]
        [InlineData("my_property", "property name")]
        [InlineData("A vertex name", "vertex name")]
        [InlineData("_private", "vertex name")]
        [InlineData("composite_idx", "index name")]
        [InlineData("name-with-hyphens", "vertex name")]
        [InlineData("1startsWithDigit", "vertex name")]
        [InlineData("String.class", "data type")]
        public void Validate_ValidIdentifier_ReturnsIdentifier(string identifier, string context)
        {
            var result = GremlinIdentifierValidator.Validate(identifier, context);
            Assert.Equal(identifier, result);
        }

        [Theory]
        [InlineData(null, "vertex name")]
        [InlineData("", "vertex name")]
        [InlineData("   ", "vertex name")]
        public void Validate_EmptyOrWhitespace_ThrowsArgumentException(string? identifier, string context)
        {
            Assert.Throws<ArgumentException>(() => GremlinIdentifierValidator.Validate(identifier!, context));
        }

        [Theory]
        [InlineData("test'; drop()", "vertex name")]
        [InlineData("name\"injection", "vertex name")]
        [InlineData("semi;colon", "vertex name")]
        [InlineData("back\\slash", "vertex name")]
        [InlineData("back`tick", "vertex name")]
        [InlineData("with(parens)", "vertex name")]
        [InlineData("with{braces}", "vertex name")]
        [InlineData("tab\there", "vertex name")]
        [InlineData("new\nline", "vertex name")]
        public void Validate_DangerousCharacters_ThrowsArgumentException(string identifier, string context)
        {
            var exception = Assert.Throws<ArgumentException>(() => GremlinIdentifierValidator.Validate(identifier, context));
            Assert.Contains("Invalid identifier", exception.Message);
            Assert.Contains(context, exception.Message);
        }
    }
}
