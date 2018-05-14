using System;
using Xunit;
using AutoFixture;
using Newtonsoft.Json.Linq;

namespace DynamicMap.Tests
{
    public class DynamicMapTest
    {
        private readonly Fixture _fixture;

        public DynamicMapTest()
        {
            _fixture = new Fixture();
            _fixture.Customize<DummyComplexClass>(x => x.With(y => y.Ancesstor, new DummyClass()
            {
                Age = _fixture.Create<int>(),
                DateOfBith = _fixture.Create<DateTime>(),
                Name = _fixture.Create<string>(),
                ParentInfo = _fixture.Create<DummyNestedClass>()
            }));
        }
        
        [Fact]
        public void Test__Basic()
        {
            // Arrange
            var obj = _fixture.Create<DummyNestedClass>();
            
            // Act
            var result = DynamicMap.Map<DummyNestedClass, DummyNestedClass>(obj);
            
            // Assert
            Assert.Equal(result, obj);
        }

        [Fact]
        public void Test__Complex()
        {
            // Arrange
            var obj = _fixture.Create<DummyClass>();

            // Act
            var result = DynamicMap.Map<DummyClass, DummyClass>(obj);

            // Assert
            Assert.Equal(result, obj);
        }
        
        [Fact]
        public void Test__BasicJObject()
        {
            // Arrange
            var obj = _fixture.Create<DummyNestedClass>();
            var json = JObject.FromObject(obj);
            
            // Act
            var result = DynamicMap.Map(typeof(DummyNestedClass), json);
            
            // Assert
            Assert.Equal(result, obj);
        }
    }
}