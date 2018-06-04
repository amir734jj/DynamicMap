using AutoFixture;
using DynamicMap.Tests.Interfaces;
using DynamicMap.Tests.Models;
using DynamicMap.Tests.Utilities;
using Xunit;

namespace DynamicMap.Tests.MapperTests
{
    public class FlatDynamicMapTest: AssertExtension, IBasicMapperTest
    {
        private readonly Fixture _fixture;

        public FlatDynamicMapTest()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Test__Basic()
        {
            // Arrange
            var obj = _fixture.Create<FlatModelSource>();
            
            // Act
            var result = DynamicMap.Map(typeof(FlatModelDestination), obj);
            
            // Assert
            Assert(obj, result, new FlatModelComparer());
        }
    }
}