using System;
using System.Collections.Generic;
using DynamicMap.Models;

namespace DynamicMap.Interfaces
{
    public interface IBaseDynamicMap
    {
        object Map(Type destinationType, Type sourceType, object sourceObj);

        IEnumerable<PropertyInfoStructSource> SourceToPropertyInfoStruct();
        
        IEnumerable<PropertyInfoStructDestination> DestinationToPropertyInfoStruct();

        object GetPropertyValue(PropertyInfoStructSource infoStructSource);

        void SetPropertyValue(PropertyInfoStructDestination infoStructSource, object value);

        Dictionary<PropertyInfoStructDestination, PropertyInfoStructSource>MappingDictionary();

        object BasicTypeConverter(Type propertyType, object value);

        object LoopBackMapper(Type destinationType, Type sourceType, object sourceObj);
    }
}