using Ivet.Model.Meta;
using Ivet.Services.Comparers;
using Ivet.TestFramework;
using Xunit;

namespace Ivet.Tests.Services.Comparers
{
    public class CompositeIndexComparerTests
    {
        private RandomGenerator _randomGenerator = new RandomGenerator();

        [Fact]
        public void EqualsTest_SameName()
        {
            // Arrange
            var sut = new CompositeIndexComparer();
            var name = RandomGenerator.RandomString();
            var a = new MetaCompositeIndex { Name = name };
            var b = new MetaCompositeIndex { Name = name };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsTest_DifferentName()
        {
            // Arrange
            var sut = new CompositeIndexComparer();
            var a = new MetaCompositeIndex { Name = RandomGenerator.RandomString() };
            var b = new MetaCompositeIndex { Name = RandomGenerator.RandomString() };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_FirstNull()
        {
            // Arrange
            var sut = new CompositeIndexComparer();
            var b = new MetaCompositeIndex { };

            // Act
            var result = sut.Equals(null, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_SecondNull()
        {
            // Arrange
            var sut = new CompositeIndexComparer();
            var a = new MetaCompositeIndex { };

            // Act
            var result = sut.Equals(a, null);

            // Assert
            Assert.False(result);
        }
    }
}
