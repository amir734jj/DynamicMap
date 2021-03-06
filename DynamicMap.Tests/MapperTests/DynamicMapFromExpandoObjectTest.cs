﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using AutoFixture;
using DynamicMap.Tests.Interfaces;
using DynamicMap.Tests.Models;
using DynamicMap.Tests.Utilities;
using Xunit;

namespace DynamicMap.Tests.MapperTests
{
    public class DynamicMapFromExpandoObjectTest: AssertExtension, IBasicMapperTest
    {
        private readonly Fixture _fixture;

        public DynamicMapFromExpandoObjectTest()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Test__Basic()
        {
            // Arrange
            var expected = _fixture.Create<ComplexModelSource>();
            var childObj = new ExpandoObject();
            var childObjDictionary = (IDictionary<string, object>) childObj;
            childObjDictionary["Name"] = expected.ParentInfo.Name;
            childObjDictionary["Age"] = expected.ParentInfo.Age;
            childObjDictionary["DateOfBith"] = expected.ParentInfo.DateOfBith;

            var rootObj = new ExpandoObject();
            var rootObjDictionary = (IDictionary<string, object>) rootObj;
            rootObjDictionary["Name"] = expected.Name;
            rootObjDictionary["Age"] = expected.Age;
            rootObjDictionary["DateOfBith"] = expected.DateOfBith;
            rootObjDictionary["ParentInfo"] = expected.ParentInfo;

            // Act
            var result = DynamicMap.Map(typeof(ComplexModelDestination), rootObj);

            // Assert
            Assert(expected, result, new ComplexModelComparer());
        }
    }
}