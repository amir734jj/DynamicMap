using System;

namespace DynamicMap.Interfaces
{
    public interface ISpecialMapper: IBaseDynamicMap, IBaseBuilder<ISpecialMapper>
    {
        /// <summary>
        /// Checks whether this ISpecialMapper can handle request map
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="sourceType"></param>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
        bool MatchingMapper(Type destinationType, Type sourceType, object sourceObj);

        /// <summary>
        /// The order of ISpecialMapper
        /// </summary>
        /// <returns></returns>
        int Order();
    }
}