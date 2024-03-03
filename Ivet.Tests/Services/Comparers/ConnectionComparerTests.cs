using Ivet.Model.Meta;
using Ivet.Services.Comparers;
using Ivet.TestFramework;
using Xunit;

namespace Ivet.Tests.Services.Comparers
{
    public class ConnectionComparerTests
    {
        private RandomGenerator _randomGenerator = new RandomGenerator();

        [Fact]
        public void EqualsTest_AllPropertiesEquals()
        {
            // Arrange
            var sut = new ConnectionComparer();
            var edge = RandomGenerator.RandomString();
            var ingoing = RandomGenerator.RandomString();
            var outgoing = RandomGenerator.RandomString();
            var a = new MetaConnection { Edge = edge, Ingoing = ingoing, Outgoing = outgoing };
            var b = new MetaConnection { Edge = edge, Ingoing = ingoing, Outgoing = outgoing };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsTest_EdgeDifferent()
        {
            // Arrange
            var sut = new ConnectionComparer();
            var ingoing = RandomGenerator.RandomString();
            var outgoing = RandomGenerator.RandomString();
            var a = new MetaConnection { Edge = RandomGenerator.RandomString(), Ingoing = ingoing, Outgoing = outgoing };
            var b = new MetaConnection { Edge = RandomGenerator.RandomString(), Ingoing = ingoing, Outgoing = outgoing };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_IngoingDifferent()
        {
            // Arrange
            var sut = new ConnectionComparer();
            var edge = RandomGenerator.RandomString();
            var outgoing = RandomGenerator.RandomString();
            var a = new MetaConnection { Edge = edge, Ingoing = RandomGenerator.RandomString(), Outgoing = outgoing };
            var b = new MetaConnection { Edge = edge, Ingoing = RandomGenerator.RandomString(), Outgoing = outgoing };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_OutgoingDifferent()
        {
            // Arrange
            var sut = new ConnectionComparer();
            var edge = RandomGenerator.RandomString();
            var ingoing = RandomGenerator.RandomString();
            var a = new MetaConnection { Edge = edge, Ingoing = ingoing, Outgoing = RandomGenerator.RandomString() };
            var b = new MetaConnection { Edge = edge, Ingoing = ingoing, Outgoing = RandomGenerator.RandomString() };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_FirstNull()
        {
            // Arrange
            var sut = new ConnectionComparer();
            var b = new MetaConnection { };

            // Act
            var result = sut.Equals(null, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_SecondNull()
        {
            // Arrange
            var sut = new ConnectionComparer();
            var a = new MetaConnection { };

            // Act
            var result = sut.Equals(a, null);

            // Assert
            Assert.False(result);
        }
    }
}
