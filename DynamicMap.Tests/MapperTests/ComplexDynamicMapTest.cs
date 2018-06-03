using AutoFixture;
using DynamicMap.Tests.Interfaces;
using DynamicMap.Tests.Models;
using Xunit;

namespace DynamicMap.Tests.MapperTests
{
    public class ComplexDynamicMapTest: IBasicMapperTest
    {
        private readonly Fixture _fixture;

        public ComplexDynamicMapTest()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Test__Basic()
        {
            // Arrange
            var obj = _fixture.Create<ComplexModel>();

            // Act
            var result = DynamicMap.Map(typeof(ComplexModel), new
            {
                Name = obj.Name,
                Age = obj.Age,
                DateOfBith = obj.DateOfBith,
                ParentInfo = new
                {
                    Name = obj.ParentInfo.Name,
                    Age = obj.ParentInfo.Age,
                    DateOfBith = obj.ParentInfo.DateOfBith
                }
            });
            
            // Assert
            Assert.Equal(obj, result);
        }
    }
}