using System;
using System.Collections.Generic;
using System.Dynamic;
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
            Assert.Equal(obj, result);
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
        
        [Fact]
        public void Test__BasicExpandoObject()
        {
            // Arrange
            var expected = new DummyClass
            {
                Name = _fixture.Create<string>(),
                Age = _fixture.Create<int>(),
                DateOfBith = _fixture.Create<DateTime>(),
                ParentInfo = new DummyNestedClass
                {
                    FatherName = _fixture.Create<string>(),
                    MotherName = _fixture.Create<string>()
                }
            };
            var childObj = new ExpandoObject();
            var childObjDictionary = (IDictionary<string, object>) childObj;
            childObjDictionary["FatherName"] = expected.ParentInfo.FatherName;
            childObjDictionary["MotherName"] = expected.ParentInfo.MotherName;

            var rootObj = new ExpandoObject();
            var rootObjDictionary = (IDictionary<string, object>) rootObj;
            rootObjDictionary["Name"] = expected.Name;
            rootObjDictionary["Age"] = expected.Age;
            rootObjDictionary["DateOfBith"] = expected.DateOfBith;
            rootObjDictionary["ParentInfo"] = expected.ParentInfo;

            // Act
            var result = DynamicMap.Map(typeof(DummyClass), rootObj) as DummyClass;
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test__BasicIEnumerableObject()
        {
            // Arrange
            var obj = _fixture.Create<DummyClassWithBasicIEnumerable>();

            // Act
            var result = DynamicMap.Map<DummyClassWithBasicIEnumerable>(typeof(DummyClassWithBasicIEnumerable), new
            {
                obj.List
            });

            // Assert
            Assert.Equal(obj, result);
        }
        
        [Fact]
        public void Test__ComplexIEnumerableObject()
        {
            // Arrange
            var obj = _fixture.Create<DummyClassComplexIEnumerable>();

            // Act
            var result = DynamicMap.Map<DummyClassComplexIEnumerable>(typeof(DummyClassComplexIEnumerable), new
            {
                obj.List
            });

            // Assert
            Assert.Equal(obj, result);
        }
    }
}