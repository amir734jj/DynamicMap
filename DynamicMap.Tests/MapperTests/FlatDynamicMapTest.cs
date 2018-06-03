using AutoFixture;
using DynamicMap.Tests.Interfaces;
using DynamicMap.Tests.Models;
using Xunit;

namespace DynamicMap.Tests.MapperTests
{
    public class FlatDynamicMapTest: IBasicMapperTest
    {
        private readonly Fixture _fixture;

        public FlatDynamicMapTest()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Test__FlatModel_TheSameTypes()
        {
            // Arrange
            var obj = _fixture.Create<FlatModel>();
            
            // Act
            var result = DynamicMap.Map(typeof(FlatModel), obj);
            
            // Assert
            Assert.Equal(obj, result);
        }
        
        [Fact]
        public void Test__Basic()
        {
            // Arrange
            var obj = _fixture.Create<FlatModel>();
            
            // Act
            var result = DynamicMap.Map(typeof(FlatModel), new
            {
                Name = obj.Name,
                Age = obj.Age,
                DateOfBith = obj.DateOfBith
            });
            
            // Assert
            Assert.Equal(obj, result);
        }
    }
}