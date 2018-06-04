using System;
using DynamicMap.Builders;

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
        /// <returns></returns>
        public static object Map(Type destinationType, object sourceObj)
        {
            return MapBuilder.RecursiveMap(destinationType, sourceObj?.GetType(), sourceObj);
        }

        /// <summary>
        /// Returns the builder
        /// </summary>
        /// <returns></returns>
        public static DynamicMapBuilder GetDynamicMapBuilder() => MapBuilder;
    }
}