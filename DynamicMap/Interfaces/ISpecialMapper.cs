using System;

namespace DynamicMap.Interfaces
{
    public interface ISpecialMapper: IBaseDynamicMap, IBaseBuilder<ISpecialMapper>
    {
        bool MatchingMapper(Type destinationType, Type sourceType, object obj);
    }
}