using AutoFixture;
using DynamicMap.Tests.Interfaces;
using DynamicMap.Tests.Models;
using Newtonsoft.Json;
using Xunit;

namespace DynamicMap.Tests.MapperTests
{
    public class DynamicMapFromJObjectTest: IBasicMapperTest
    {
        private readonly Fixture _fixture;

        public DynamicMapFromJObjectTest()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Test__Basic()
        {
            // Arrange
            var obj = _fixture.Create<ComplexModel>();
            var json = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(obj));

            // Act
            var result = DynamicMap.Map(typeof(ComplexModel), json);

            // Assert
            Assert.Equal(obj, result);
        }
    }
}