using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DynamicMap.BaseMapper;
using DynamicMap.Extensions;
using DynamicMap.Interfaces;
using DynamicMap.Models;

namespace DynamicMap.SpecialMappers
{
    public class FromExpandoObjectMapper: BaseDynamicMap, ISpecialMapper
    {
        /// <summary>
        /// Needed to show this mapper can handle ExpandoObjects
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="sourceType"></param>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
        public bool MatchingMapper(Type destinationType, Type sourceType, object sourceObj)
        {
            switch (sourceObj)
            {
                case IDictionary<string, object> _ when !typeof(IDictionary<string, object>).IsAssignableFrom(destinationType):
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// The order of this ISpecialMapper
        /// </summary>
        /// <returns></returns>
        public int Order() => 2;

        /// <summary>
        /// Instantiate this ISpecialMapper
        /// </summary>
        /// <returns></returns>
        public new ISpecialMapper New() => new FromExpandoObjectMapper();

        /// <summary>
        /// Safely extract source properties
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public override IEnumerable<PropertyInfoStructSource> SourceToPropertyInfoStruct()
        {
            switch (_sourceObj)
            {
                case IDictionary<string, object> dictionary:
                    return dictionary.Select(x =>
                    {
                        return new PropertyInfoStructSource
                        {
                            Name = x.Key,
                            PropertyType = x.Value?.GetType(),
                            Getter = () => dictionary[x.Key],
                            IsComplexType = x.Value != null && !x.Value.GetType().IsPrimitiveSystemType()
                        };
                    });
                default:
                    throw new ArgumentException();
            }
        }
    }
}