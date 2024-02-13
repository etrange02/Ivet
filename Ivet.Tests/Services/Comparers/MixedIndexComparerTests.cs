using Ivet.Model.Meta;
using Ivet.Services.Comparers;
using Ivet.TestFramework;
using Xunit;

namespace Ivet.Tests.Services.Comparers
{
    public class MixedIndexComparerTests
    {
        private RandomGenerator _randomGenerator = new RandomGenerator();

        [Fact]
        public void EqualsTest_SameName()
        {
            // Arrange
            var sut = new MixedIndexComparer();
            var name = _randomGenerator.RandomString();
            var a = new MetaMixedIndex { Name = name };
            var b = new MetaMixedIndex { Name = name };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsTest_DifferentName()
        {
            // Arrange
            var sut = new MixedIndexComparer();
            var a = new MetaMixedIndex { Name = _randomGenerator.RandomString() };
            var b = new MetaMixedIndex { Name = _randomGenerator.RandomString() };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_FirstNull()
        {
            // Arrange
            var sut = new MixedIndexComparer();
            var b = new MetaMixedIndex { };

            // Act
            var result = sut.Equals(null, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_SecondNull()
        {
            // Arrange
            var sut = new MixedIndexComparer();
            var a = new MetaMixedIndex { };

            // Act
            var result = sut.Equals(a, null);

            // Assert
            Assert.False(result);
        }
    }
}
