using AutoFixture;
using Xunit;

namespace DynamicMap.Tests.MapperTests
{
    public class PrimitiveMapTest
    {
        private readonly Fixture _fixture;

        public PrimitiveMapTest()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Test__int_to_string()
        {
            // Arrange
            var value = _fixture.Create<int>();

            // Act
            var result = DynamicMap.Map<string>(value);

            // Assert
            Assert.Equal(value.ToString(), result);
        }
        
        [Fact]
        public void Test__string_to_int()
        {
            // Arrange
            var value = _fixture.Create<int>();

            // Act
            var result = DynamicMap.Map<int>(value.ToString());

            // Assert
            Assert.Equal(value, result);
        }
        
        [Fact]
        public void Test__int_to_decimal()
        {
            // Arrange
            var value = _fixture.Create<int>();

            // Act
            var result = DynamicMap.Map<decimal>(value);

            // Assert
            Assert.Equal(new decimal(value), result);
        }
        
        [Fact]
        public void Test__decimal_to_int()
        {
            // Arrange
            var value = _fixture.Create<decimal>();

            // Act
            var result = DynamicMap.Map<int>(value);

            // Assert
            Assert.Equal((int) value, result);
        }
    }
}