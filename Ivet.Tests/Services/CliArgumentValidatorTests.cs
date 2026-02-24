using Ivet.Services;
using Xunit;

namespace Ivet.Tests.Services
{
    public class CliArgumentValidatorTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(8182)]
        [InlineData(65535)]
        public void ValidatePort_ValidPort_ReturnsPort(int port)
        {
            var result = CliArgumentValidator.ValidatePort(port);
            Assert.Equal(port, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(65536)]
        [InlineData(100000)]
        public void ValidatePort_InvalidPort_ThrowsArgumentOutOfRangeException(int port)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CliArgumentValidator.ValidatePort(port));
        }

        [Theory]
        [InlineData("v1.0")]
        [InlineData("sprint-42")]
        [InlineData("2024Q1")]
        public void ValidateSprintNo_ValidSprintNo_ReturnsSprintNo(string sprintNo)
        {
            var result = CliArgumentValidator.ValidateSprintNo(sprintNo);
            Assert.Equal(sprintNo, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ValidateSprintNo_NullOrEmpty_ReturnsInput(string? sprintNo)
        {
            var result = CliArgumentValidator.ValidateSprintNo(sprintNo);
            Assert.Equal(sprintNo, result);
        }

        [Theory]
        [InlineData("../../etc")]
        [InlineData("sprint..evil")]
        public void ValidateSprintNo_PathTraversal_ThrowsArgumentException(string sprintNo)
        {
            var ex = Assert.Throws<ArgumentException>(() => CliArgumentValidator.ValidateSprintNo(sprintNo));
            Assert.Contains("path traversal", ex.Message);
        }

        [Theory]
        [InlineData("/etc/passwd")]
        [InlineData("C:\\Windows")]
        public void ValidateSprintNo_AbsolutePath_ThrowsArgumentException(string sprintNo)
        {
            var ex = Assert.Throws<ArgumentException>(() => CliArgumentValidator.ValidateSprintNo(sprintNo));
            Assert.Contains("path traversal", ex.Message);
        }

        [Fact]
        public void ValidateTimeout_Null_ReturnsNull()
        {
            var result = CliArgumentValidator.ValidateTimeout(null);
            Assert.Null(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        [InlineData(300000)]
        public void ValidateTimeout_ValidValue_ReturnsValue(long timeout)
        {
            var result = CliArgumentValidator.ValidateTimeout(timeout);
            Assert.Equal(timeout, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-1000)]
        public void ValidateTimeout_InvalidValue_ThrowsArgumentOutOfRangeException(long timeout)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CliArgumentValidator.ValidateTimeout(timeout));
        }
    }
}
