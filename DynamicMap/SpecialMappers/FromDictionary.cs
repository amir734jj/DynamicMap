using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DynamicMap.BaseMapper;
using DynamicMap.Extensions;
using DynamicMap.Interfaces;
using DynamicMap.Models;

namespace DynamicMap.SpecialMappers
{
    public class FromDictionary: BaseDynamicMap, ISpecialMapper
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
            if (destinationType.IsDictionaryType(out _, out _) && sourceType.IsDictionaryType(out _, out _))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The order of this ISpecialMapper
        /// </summary>
        /// <returns></returns>
        public int Order() => 3;

        /// <summary>
        /// Instantiate this ISpecialMapper
        /// </summary>
        /// <returns></returns>
        public new ISpecialMapper New() => new FromDictionary();

        /// <summary>
        /// Safely extract source properties
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public override IEnumerable<PropertyInfoStructSource> SourceToPropertyInfoStruct()
        {
            switch (_sourceObj)
            {
                case IDictionary dictionary:
                    var retVal = new List<PropertyInfoStructSource>();

                    foreach (DictionaryEntry entry in dictionary)
                    {
                        retVal.Add(new PropertyInfoStructSource
                        {
                            Name = entry.Key.ToString(),
                            PropertyType = entry.Value.GetType(),
                            Getter = () => entry.Value,
                            IsComplexType = entry.Value != null && !entry.Value.GetType().IsPrimitiveSystemType()
                        });
                    }
                    return retVal;
                default:
                    throw new ArgumentException();
            }
        }
        
        /// <summary>
        /// Treat destination IEnumerable as a field
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<PropertyInfoStructDestination> DestinationToPropertyInfoStruct()
        {
            switch (_sourceObj)
            {
                case IDictionary dictionary:
                    var retVal = new List<PropertyInfoStructDestination>();

                    foreach (DictionaryEntry entry in dictionary)
                    {
                        retVal.Add(new PropertyInfoStructDestination
                        {
                            Name = entry.Key.ToString(),
                            PropertyType = entry.Value.GetType(),
                            Setter = value => ((IDictionary) _result)[entry.Key] = value
                        });
                    }

                    return retVal;
                default:
                    throw new ArgumentException();
            }
        }
    }
}