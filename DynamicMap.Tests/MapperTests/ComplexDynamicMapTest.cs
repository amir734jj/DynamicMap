using AutoFixture;
using DynamicMap.Tests.Interfaces;
using DynamicMap.Tests.Models;
using DynamicMap.Tests.Utilities;
using Xunit;

namespace DynamicMap.Tests.MapperTests
{
    public class ComplexDynamicMapTest: AssertExtension, IBasicMapperTest
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
            var obj = _fixture.Create<ComplexModelSource>();
            
            // Act
            var result = DynamicMap.Map(typeof(ComplexModelDestination), obj);
            
            // Assert
            Assert(obj, result, new ComplexModelComparer());
        }
    }
}