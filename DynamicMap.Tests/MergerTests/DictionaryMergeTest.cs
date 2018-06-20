using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using DynamicMap.Tests.Interfaces;
using DynamicMap.Tests.Utilities;
using Xunit;

namespace DynamicMap.Tests.MergerTests
{
    public class DictionaryMergeTest: AssertExtension, IBasicMapperTest
    {
        private readonly Fixture _fixture;

        public DictionaryMergeTest()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Test__Basic()
        {
            // Arrange 
            var objs = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    {"prop1", _fixture.Create<string>()}
                },
                new Dictionary<string, string>
                {
                    {"prop2", _fixture.Create<string>()}
                }
            };
            
            var expected = new Dictionary<string, string>
            {
                {"prop1", objs.First()["prop1"]},
                {"prop2", objs.Last()["prop2"]}
            };

            // Act
            var result = DynamicMap.Merge<Dictionary<string, string>>(objs);

            // Assert
            Xunit.Assert.Equal(expected, result);
        }

        [Fact]
        public void Test__Complex()
        {
            // Arrange 
            var objs = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"prop", _fixture.Create<string>()}
                }
            };
            
            var expected = new Dictionary<string, object>
            {
                {"prop", objs.First()["prop"]}
            };

            // Act
            var result = DynamicMap.Merge<Dictionary<string, object>>(objs);

            // Assert
            Xunit.Assert.Equal(expected, result);
        }
        
        [Fact]
        public void Test__VeryComplex()
        {
            // Arrange 
            var objs = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"prop", new Dictionary<string, object> { { "nestedProp1", _fixture.Create<string>() }, { "nestedProp", _fixture.Create<string>() }}}
                },
                new Dictionary<string, object>
                {
                    {"prop", new Dictionary<string, object> { { "nestedProp2", _fixture.Create<string>() }, { "nestedProp", _fixture.Create<string>() }}}
                }
            };
            
            var expected = new Dictionary<string, object>
            {
                {"prop", objs.First()["prop"]}
            };

            // Act
            var result = DynamicMap.Merge<Dictionary<string, object>>(objs);

            // Assert
            Xunit.Assert.Equal(expected, result);
        } 
    }
}