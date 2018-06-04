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
        /// <summary>
        /// Instantiate this ISpecialMapper
        /// </summary>
        /// <returns></returns>
        public new ISpecialMapper New() => new FromJObjectMapper();

        /// <summary>
        /// Needed to show this mapper can handle IEnumerable
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="sourceType"></param>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
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

        /// <summary>
        /// The order of this ISpecialMapper
        /// </summary>
        /// <returns></returns>
        public int Order() => 1;

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

        /// <summary>
        /// Checks JProperty is complex type or not
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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