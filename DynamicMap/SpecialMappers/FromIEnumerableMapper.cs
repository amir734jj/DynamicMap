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
        /// <summary>
        /// Virtual property name
        /// </summary>
        private readonly string _propertyName = Guid.NewGuid().ToString("n").Substring(0, 8);
        
        /// <summary>
        /// Instantiate this ISpecialMapper
        /// </summary>
        /// <returns></returns>
        public new ISpecialMapper New() => new FromIEnumerableMapper();

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
                case IEnumerable _:
                    return true;
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Order of this ISpecialMapper, needed as JObject and ExpandoObject are both IEnumerables too
        /// </summary>
        /// <returns></returns>
        public int Order() => 4;

        /// <summary>
        /// Treat source IEnumerable as a field
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<PropertyInfoStructSource> SourceToPropertyInfoStruct()
        {
            return new List<PropertyInfoStructSource>
            {
                new PropertyInfoStructSource
                {
                    Name = _propertyName,
                    Getter = () =>
                    {
                        var source = (IEnumerable) _sourceObj;
                        var result = (IEnumerable) _destinationType.Instantiate();
                        
                        foreach (var nestedObj in source)
                        {
                            // ReSharper disable once PossibleMultipleEnumeration
                            AddToIEnumerable(result, LoopBackMapper(_destinationType.GetGenericType(), _sourceType.GetGenericType(), nestedObj));
                        }

                        return result;
                    }
                }
            };
        }

        /// <summary>
        /// Treat destination IEnumerable as a field
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<PropertyInfoStructDestination> DestinationToPropertyInfoStruct()
        {
            return new List<PropertyInfoStructDestination>
            {
                new PropertyInfoStructDestination
                {
                    Name = _propertyName,
                    Setter = value => _result = value
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