using System;
using AutoFixture;
using DynamicMap.BaseMapper;
using DynamicMap.Interfaces;
using DynamicMap.Tests.Interfaces;
using Xunit;

namespace DynamicMap.Tests.BuilderTests
{
    internal static class Flag
    {
        public static bool FlagValue { get; set; } = false;
    }
    
    internal class CustomClass
    {
        public string CustomProp { get; set; }
    }

    internal class CustomClassSpecialMapper : BaseDynamicMap, ISpecialMapper
    {
        public new ISpecialMapper New() => new CustomClassSpecialMapper();

        public bool MatchingMapper(Type destinationType, Type sourceType, object sourceObj)
        {
            if (sourceType == typeof(CustomClass))
            {
                Flag.FlagValue = true;
                return true;
            }

            return false;
        }

        public int Order() => 4;
    }
    
    public class CustomProfileTest: IBasicMapperTest
    {
        private readonly Fixture _fixture;

        public CustomProfileTest()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Test__Basic()
        {
            // Arrange
            var obj = _fixture.Create<CustomClass>();
            
            // Act
            DynamicMap.GetDynamicMapBuilder().RegisterCustomMapper(new CustomClassSpecialMapper());            
            var result = (CustomClass) DynamicMap.Map(typeof(CustomClass), new
            {
                CustomProp = obj.CustomProp
            });
            
            // Assert
            Assert.True(obj.CustomProp == result.CustomProp && Flag.FlagValue);
        }
    }
}