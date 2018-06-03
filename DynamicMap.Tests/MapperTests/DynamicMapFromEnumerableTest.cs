using AutoFixture;
using DynamicMap.Tests.Interfaces;
using DynamicMap.Tests.Models;
using Newtonsoft.Json;
using Xunit;

namespace DynamicMap.Tests.MapperTests
{
    public class DynamicMapFromEnumerableTest: IBasicMapperTest
    {
        private readonly Fixture _fixture;

        public DynamicMapFromEnumerableTest()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Test__Basic()
        {
            // Arrange
            var obj = _fixture.Create<EnumerableModel>();
            var json = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(obj));

            // Act
            var result = DynamicMap.Map(typeof(EnumerableModel), json);

            // Assert
            Assert.Equal(obj, result);
        }
    }
}