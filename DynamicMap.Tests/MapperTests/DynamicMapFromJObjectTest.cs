using AutoFixture;
using DynamicMap.Tests.Interfaces;
using DynamicMap.Tests.Models;
using DynamicMap.Tests.Utilities;
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
            var obj = _fixture.Create<ComplexModelSource>();
            var json = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(obj));

            // Act
            var result = DynamicMap.Map(typeof(ComplexModelSource), json);

            // Assert
            Assert.Equal(obj, result);
        }
    }
}