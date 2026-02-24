using Ivet.Services;
using Xunit;

namespace Ivet.Tests.Services
{
    public class GremlinScriptValidatorTests
    {
        [Theory]
        [InlineData("mgmt = graph.openManagement()")]
        [InlineData("mgmt.makeVertexLabel('Person').make()")]
        [InlineData("mgmt.makePropertyKey('name').dataType(String.class).make()")]
        [InlineData("mgmt.commit()")]
        public void Validate_ValidSchemaScript_DoesNotThrow(string script)
        {
            GremlinScriptValidator.Validate(script);
        }

        [Fact]
        public void Validate_DropPattern_ThrowsInvalidOperationException()
        {
            var ex = Assert.Throws<InvalidOperationException>(
                () => GremlinScriptValidator.Validate("g.V().drop()"));
            Assert.Contains(".drop()", ex.Message);
        }

        [Fact]
        public void Validate_SystemExit_ThrowsInvalidOperationException()
        {
            var ex = Assert.Throws<InvalidOperationException>(
                () => GremlinScriptValidator.Validate("System.exit(0)"));
            Assert.Contains("System.exit", ex.Message);
        }

        [Fact]
        public void Validate_RuntimeGetRuntime_ThrowsInvalidOperationException()
        {
            var ex = Assert.Throws<InvalidOperationException>(
                () => GremlinScriptValidator.Validate("Runtime.getRuntime().exec('cmd')"));
            Assert.Contains("Runtime.getRuntime", ex.Message);
        }

        [Fact]
        public void Validate_JavaIo_ThrowsInvalidOperationException()
        {
            var ex = Assert.Throws<InvalidOperationException>(
                () => GremlinScriptValidator.Validate("new java.io.FileInputStream('/etc/passwd')"));
            Assert.Contains("java.io.", ex.Message);
        }

        [Fact]
        public void Validate_CaseInsensitive_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(
                () => GremlinScriptValidator.Validate("g.V().DROP()"));
        }
    }
}
