using System;
using System.Collections.Generic;
using System.Linq;
using DynamicMap.Builders;
using DynamicMap.Enums;
using DynamicMap.Extensions;
using DynamicMap.Utilities;

namespace DynamicMap
{
    public static class DynamicMap
    {
        private static readonly DynamicMapBuilder MapBuilder = new DynamicMapBuilder();

        /// <summary>
        /// Maps an object of type object to given anonymous
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="sourceObj"></param>
        /// <param name="destinationObj"></param>
        /// <param name="mapingMode"></param>
        /// <returns></returns>
        public static object Map(Type destinationType, object sourceObj, object destinationObj = null, MappingMode mapingMode = MappingMode.Map)
        {
            return MapBuilder.RecursiveMap(destinationType, sourceObj?.GetType(), sourceObj, destinationObj, mapingMode);
        }

        /// <summary>
        /// Maps an object of type object to given static type
        /// </summary>
        /// <param name="sourceObj"></param>
        /// <param name="destinationObj"></param>
        /// <param name="mapingMode"></param>
        /// <returns></returns>
        public static T Map<T>(object sourceObj, object destinationObj = null, MappingMode mapingMode = MappingMode.Map)
        {
            return (T) MapBuilder.RecursiveMap(typeof(T), sourceObj?.GetType(), sourceObj, destinationObj, mapingMode);
        }

        /// <summary>
        /// Recursive merge given dynamic type
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static object Merge(Type destinationType, params ParamsWrapper<object>[] objects)
        {
            return MapBuilder.RecursiveMerge(destinationType, objects.SelectMany(x => x).ToArray());
        }

        /// <summary>
        /// Recursive merge given static type
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static T Merge<T>(params ParamsWrapper<object>[] objects)
        {
            var @params = objects.SelectMany(x => x).ToArray();

            if (@params.Length == 1 && @params.Select(x => x.GetType()).ToHashSet().Count == 1)
            {
                @params = @params.Cast<IEnumerable<object>>().First().ToArray();
            }
            
            return (T) MapBuilder.RecursiveMerge(typeof(T), @params);
        }

        /// <summary>
        /// Returns the builder
        /// </summary>
        /// <returns></returns>
        public static DynamicMapBuilder GetDynamicMapBuilder() => MapBuilder;
    }
}