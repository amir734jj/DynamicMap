using System;
using System.Collections.Generic;
using System.Linq;
using DynamicMap.BaseMapper;
using DynamicMap.Interfaces;
using DynamicMap.Models;
using Newtonsoft.Json.Linq;

namespace DynamicMap.SpecialMappers
{
    public class FromJObjectMapper: BaseDynamicMap, ISpecialMapper
    {
        public new ISpecialMapper New() => new FromJObjectMapper();

        public bool MatchingMapper(Type destinationType, Type sourceType, object sourceObj)
        {
            switch (sourceObj)
            {
                case JObject _:
                    return true;
                default:
                    return false;
            }
        }

        public override IEnumerable<PropertyInfoStructSource> SourceToPropertyInfoStruct()
        {
            return ((JObject) _sourceObj).Properties().Select(x => new PropertyInfoStructSource
            {
                Name = x.Name,
                Getter = () => x.Value.ToObject<object>(),
                IsComplexType = IsComplexType(x.Value.ToObject<object>()),
                PropertyType = x.Type.GetType()
            });
        }

        private static bool IsComplexType(object value)
        {
            switch (value)
            {
                case JObject _:
                    return true;
                default:
                    return false;
            }
        }
    }
}