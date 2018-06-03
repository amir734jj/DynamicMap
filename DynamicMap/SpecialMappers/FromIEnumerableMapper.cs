using System;
using DynamicMap.BaseMapper;
using DynamicMap.Interfaces;

namespace DynamicMap.SpecialMappers
{
    public class FromIEnumerableMapper: BaseDynamicMap, ISpecialMapper
    {
        public new ISpecialMapper New() => new FromIEnumerableMapper();

        public bool MatchingMapper(Type destinationType, Type sourceType, object obj)
        {
            throw new NotImplementedException();
        }
    }
}