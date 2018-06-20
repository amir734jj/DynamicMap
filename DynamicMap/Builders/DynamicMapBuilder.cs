using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DynamicMap.BaseMapper;
using DynamicMap.Enums;
using DynamicMap.Extensions;
using DynamicMap.Interfaces;
using static DynamicMap.Utilities.LambdaHelper;

namespace DynamicMap.Builders
{
    public class DynamicMapBuilder: BaseBuilder<DynamicMapBuilder>
    {
        /// <summary>
        /// Get base dynamic mapper
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private readonly BaseDynamicMap _baseMapper = new BaseDynamicMap();

        /// <summary>
        /// Get special mappers
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private readonly HashSet<ISpecialMapper> _specialMappers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => !x.IsInterface)
            .Where(x => typeof(ISpecialMapper).IsAssignableFrom(x))
            .Select(x => x.Instantiate())
            .Cast<ISpecialMapper>()
            .OrderBy(x => x.Order())
            .ToHashSet();
            
        /// <summary>
        /// Registers a custom mapper
        /// </summary>
        /// <param name="specialMapper"></param>
        /// <returns></returns>
        public DynamicMapBuilder RegisterCustomMapper(ISpecialMapper specialMapper) => Run(() => _specialMappers.Add(specialMapper), this);

        /// <summary>
        /// Recurisve mapper
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="sourceType"></param>
        /// <param name="sourceObj"></param>
        /// <param name="destinationObj"></param>
        /// <param name="mappingMode"></param>
        /// <returns></returns>
        public object RecursiveMap(Type destinationType, Type sourceType, object sourceObj, object destinationObj = null, MappingMode mappingMode = MappingMode.Map)
        {
            // test for edge cases
            if (ValidateEdgeCases(destinationType, sourceType, sourceObj, out var result)) return result;

            var mapper = _specialMappers.FirstOrDefault(x => x.MatchingMapper(destinationType, destinationType, sourceObj));

            return mapper != null ? mapper.New().Map(mappingMode, destinationType, sourceType, sourceObj, destinationObj) : _baseMapper.New().Map(mappingMode, destinationType, sourceType, sourceObj, destinationObj);
        }

        /// <summary>
        /// Recursively merge the object
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public object RecursiveMerge(Type destinationType, object[] objects)
        {
            // if type is null just return null
            if (destinationType == null) return null;

            if (objects == null || !objects.Any()) return null;

            var destinationObj = destinationType.Instantiate();

            objects.Where(x => x != null).ForEach(x =>
            {
                destinationObj = RecursiveMap(destinationType, x.GetType(), x, destinationObj, MappingMode.Merge);
            });

            return destinationObj;
        }
        
        /// <summary>
        /// Special case mappings
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="sourceType"></param>
        /// <param name="sourceObj"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static bool ValidateEdgeCases(Type destinationType, Type sourceType, object sourceObj, out object result)
        {
            // instantiate destination type
            result = null;
            
            // if type is null just return null
            if (destinationType == null) return true;
            
            // if type is null just return null
            if (sourceType == null) return true;
            
            // if object is null, then return default
            if (sourceObj == null) return true;

            // no mapping is needed
            // ReSharper disable once InvertIf
//            if (sourceType == destinationType)
//            {
//                result = sourceObj;
//                return true;
//            }

            return false;
        }
    }
}