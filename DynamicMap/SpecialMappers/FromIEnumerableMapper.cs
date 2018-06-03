using System;
using System.Collections;
using System.Collections.Generic;
using DynamicMap.BaseMapper;
using DynamicMap.Extensions;
using DynamicMap.Interfaces;
using DynamicMap.Models;

namespace DynamicMap.SpecialMappers
{
    public class FromIEnumerableMapper: BaseDynamicMap, ISpecialMapper
    {
        public new ISpecialMapper New() => new FromIEnumerableMapper();

        public bool MatchingMapper(Type destinationType, Type sourceType, object sourceObj)
        {
            switch (sourceObj)
            {
                case IEnumerable enumerable:
                    return true;
                default:
                    return false;
            }
        }

        public override IEnumerable<PropertyInfoStructSource> SourceToPropertyInfoStruct()
        {
            return new List<PropertyInfoStructSource>
            {
                new PropertyInfoStructSource
                {
                    Getter = () =>
                    {
                        var source = (IEnumerable) _sourceObj;
                        var result = (IEnumerable) _destinationType.Instantiate();
                        
                        foreach (var nestedObj in source)
                        {
                            // ReSharper disable once PossibleMultipleEnumeration
                            AddToIEnumerable(result, nestedObj);
                        }

                        return result;
                    }
                }
            };
        }

        /// <summary>
        /// Add value to IEnumerable
        /// TODO: add more IEnumerable interfaces
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="value"></param>
        private static void AddToIEnumerable(IEnumerable enumerable, object value)
        {
            switch (enumerable)
            {
                case IList list:
                    list.Add(value);
                    break;
            }
        }
    }
}