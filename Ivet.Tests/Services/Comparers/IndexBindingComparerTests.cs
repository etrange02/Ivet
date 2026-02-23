using Ivet.Model.Meta;
using Ivet.Services.Comparers;
using Ivet.TestFramework;
using Xunit;

namespace Ivet.Tests.Services.Comparers
{
    public class IndexBindingComparerTests
    {
        private RandomGenerator _randomGenerator = new RandomGenerator();

        [Fact]
        public void EqualsTest_AllPropertiesEquals()
        {
            // Arrange
            var sut = new IndexBindingComparer();
            var indexName = RandomGenerator.RandomString();
            var propertyName = RandomGenerator.RandomString();
            var a = new MetaIndexBinding { IndexName = indexName, PropertyName = propertyName };
            var b = new MetaIndexBinding { IndexName = indexName, PropertyName = propertyName };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsTest_EdgeDifferent()
        {
            // Arrange
            var sut = new IndexBindingComparer();
            var indexName = RandomGenerator.RandomString();
            var a = new MetaIndexBinding { IndexName = indexName, PropertyName = RandomGenerator.RandomString() };
            var b = new MetaIndexBinding { IndexName = indexName, PropertyName = RandomGenerator.RandomString() };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_PropertyNameDifferent()
        {
            // Arrange
            var sut = new IndexBindingComparer();
            var indexName = RandomGenerator.RandomString();
            var a = new MetaIndexBinding { IndexName = indexName, PropertyName = RandomGenerator.RandomString() };
            var b = new MetaIndexBinding { IndexName = indexName, PropertyName = RandomGenerator.RandomString() };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_FirstNull()
        {
            // Arrange
            var sut = new IndexBindingComparer();
            var b = new MetaIndexBinding { };

            // Act
            var result = sut.Equals(null, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_SecondNull()
        {
            // Arrange
            var sut = new IndexBindingComparer();
            var a = new MetaIndexBinding { };

            // Act
            var result = sut.Equals(a, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_IndexNameDifferent()
        {
            // Arrange
            var sut = new IndexBindingComparer();
            var propertyName = RandomGenerator.RandomString();
            var a = new MetaIndexBinding { IndexName = RandomGenerator.RandomString(), PropertyName = propertyName };
            var b = new MetaIndexBinding { IndexName = RandomGenerator.RandomString(), PropertyName = propertyName };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCodeTest_SameIndexNameSameHash()
        {
            // Arrange
            var sut = new IndexBindingComparer();
            var indexName = RandomGenerator.RandomString();
            var a = new MetaIndexBinding { IndexName = indexName, PropertyName = RandomGenerator.RandomString() };
            var b = new MetaIndexBinding { IndexName = indexName, PropertyName = RandomGenerator.RandomString() };

            // Act & Assert
            Assert.Equal(sut.GetHashCode(a), sut.GetHashCode(b));
        }

        [Fact]
        public void GetHashCodeTest_DifferentIndexNameDifferentHash()
        {
            // Arrange
            var sut = new IndexBindingComparer();
            var a = new MetaIndexBinding { IndexName = RandomGenerator.RandomString() };
            var b = new MetaIndexBinding { IndexName = RandomGenerator.RandomString() };

            // Act & Assert
            Assert.NotEqual(sut.GetHashCode(a), sut.GetHashCode(b));
        }
    }
}
