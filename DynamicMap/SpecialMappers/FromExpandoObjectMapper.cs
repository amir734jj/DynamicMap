using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DynamicMap.BaseMapper;
using DynamicMap.Interfaces;
using DynamicMap.Models;

namespace DynamicMap.SpecialMappers
{
    public class FromExpandoObjectMapper: BaseDynamicMap, ISpecialMapper
    {
        public bool MatchingMapper(Type destinationType, Type sourceType, object obj)
        {
            switch (obj)
            {
                case IDictionary<string, object> _ when destinationType != typeof(IDictionary<string, object>):
                    return true;
                default:
                    return false;
            }
        }

        public new ISpecialMapper New() => new FromExpandoObjectMapper();

        public override IEnumerable<PropertyInfoStructSource> SourceToPropertyInfoStruct()
        {
            switch (_sourceObj)
            {
                case IDictionary<string, object> dictionary:
                    return dictionary.Select(x => new PropertyInfoStructSource
                    {
                        Name = x.Key,
                        PropertyType = x.Value?.GetType(),
                        Getter = () => dictionary[x.Key]
                    });
                default:
                    throw new ArgumentException();
            }
        }
    }
}