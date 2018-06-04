using System.Collections.Generic;
using AutoFixture;
using DynamicMap.Tests.Interfaces;
using Xunit;

namespace DynamicMap.Tests.BuilderTests
{
    public class TheSameTypeTest: IBasicMapperTest
    {
        private readonly Fixture _fixture;

        public TheSameTypeTest()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Test__Basic()
        {
            // Arrange
            var obj = _fixture.Create<List<string>>();

            // Act
            var result = DynamicMap.Map(typeof(List<string>), obj);

            // Assett
            Assert.Equal(obj, result);
        }
    }
}