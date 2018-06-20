using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using DynamicMap.Tests.Interfaces;
using DynamicMap.Tests.Models;
using DynamicMap.Tests.Utilities;
using Xunit;

namespace DynamicMap.Tests.MergerTests
{
    public class FlatDynamicMergeTest: AssertExtension, IBasicMapperTest
    {
        private readonly Fixture _fixture;

        public FlatDynamicMergeTest()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Test__Basic()
        {
            // Arrange
            var  objs = new List<MergingModel> {
                _fixture.Build<MergingModel>()
                    .With(x => x.StrProp1, _fixture.Create<string>())
                    .Without(x => x.StrProp2)
                    .Without(x => x.StrProp3)
                    .With(x => x.DateTimeProp1, _fixture.Create<DateTime>())
                    .Without(x => x.DateTimeProp2)
                    .Without(x => x.DateTimeProp3)
                    .Create(),
                _fixture.Build<MergingModel>()
                    .Without(x => x.StrProp1)
                    .With(x => x.StrProp2, _fixture.Create<string>())
                    .Without(x => x.StrProp3)
                    .Without(x => x.DateTimeProp1)
                    .With(x => x.DateTimeProp2, _fixture.Create<DateTime>())
                    .Without(x => x.DateTimeProp3)
                    .Create(),
                _fixture.Build<MergingModel>()
                    .Without(x => x.StrProp1)
                    .Without(x => x.StrProp2)
                    .With(x => x.StrProp3, _fixture.Create<string>())
                    .Without(x => x.DateTimeProp1)
                    .Without(x => x.DateTimeProp2)
                    .With(x => x.DateTimeProp3, _fixture.Create<DateTime>())
                    .Create()
            };

            var expected = new MergingModel
            {
                StrProp1 = objs[0].StrProp1,
                StrProp2 = objs[1].StrProp2,
                StrProp3 = objs[2].StrProp3,
                DateTimeProp1 = objs[0].DateTimeProp1,
                DateTimeProp2 = objs[1].DateTimeProp2,
                DateTimeProp3 = objs[2].DateTimeProp3,
            };
            
            // Act
            var result = DynamicMap.Merge<MergingModel>(objs);
            
            // Assert
            Assert(expected, result, new MergingModelComparer());
        }
    }
}