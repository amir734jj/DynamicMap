using AutoFixture;
using DynamicMap.Tests.Interfaces;
using DynamicMap.Tests.Models;
using DynamicMap.Tests.Utilities;
using Xunit;

namespace DynamicMap.Tests.MapperTests
{
    public class DynamicMapFromEnumerableTest: AssertExtension, IBasicMapperTest
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
            var obj = _fixture.Create<EnumerableModelSource>();
            
            // Act
            var result = DynamicMap.Map(typeof(EnumerableModelDestination), obj);
            
            // Assert
            Assert(obj, result, new EnumerableModelComparer());
        }
    }
}