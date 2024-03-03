using Ivet.Model.Meta;
using Ivet.Services.Comparers;
using Ivet.TestFramework;
using Xunit;

namespace Ivet.Tests.Services.Comparers
{
    public class PropertyBindingComparerTests
    {
        private RandomGenerator _randomGenerator = new RandomGenerator();

        [Fact]
        public void EqualsTest_AllPropertiesEquals()
        {
            // Arrange
            var sut = new PropertyBindingComparer();
            var entity = RandomGenerator.RandomString();
            var name = RandomGenerator.RandomString();
            var a = new MetaPropertyBinding { Entity = entity, Name = name };
            var b = new MetaPropertyBinding { Entity = entity, Name = name };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsTest_EntityDifferent()
        {
            // Arrange
            var sut = new PropertyBindingComparer();
            var name = RandomGenerator.RandomString();
            var a = new MetaPropertyBinding { Entity = RandomGenerator.RandomString(), Name = name };
            var b = new MetaPropertyBinding { Entity = RandomGenerator.RandomString(), Name = name };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_NameDifferent()
        {
            // Arrange
            var sut = new PropertyBindingComparer();
            var entity = RandomGenerator.RandomString();
            var a = new MetaPropertyBinding { Entity = entity, Name = RandomGenerator.RandomString() };
            var b = new MetaPropertyBinding { Entity = entity, Name = RandomGenerator.RandomString() };

            // Act
            var result = sut.Equals(a, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_FirstNull()
        {
            // Arrange
            var sut = new PropertyBindingComparer();
            var b = new MetaPropertyBinding { };

            // Act
            var result = sut.Equals(null, b);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsTest_SecondNull()
        {
            // Arrange
            var sut = new PropertyBindingComparer();
            var a = new MetaPropertyBinding { };

            // Act
            var result = sut.Equals(a, null);

            // Assert
            Assert.False(result);
        }
    }
}
