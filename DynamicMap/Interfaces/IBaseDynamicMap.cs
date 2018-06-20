using System;
using System.Collections.Generic;
using DynamicMap.Enums;
using DynamicMap.Models;

namespace DynamicMap.Interfaces
{
    public interface IBaseDynamicMap
    {
        object Map(MappingMode mappingMode, Type destinationType, Type sourceType, object sourceObj, object destinationObj = null);

        IEnumerable<PropertyInfoStructSource> SourceToPropertyInfoStruct();
        
        IEnumerable<PropertyInfoStructDestination> DestinationToPropertyInfoStruct();

        object GetPropertyValue(PropertyInfoStructSource infoStructSource);

        void SetPropertyValue(PropertyInfoStructDestination infoStructSource, object value);

        Dictionary<PropertyInfoStructDestination, PropertyInfoStructSource>MappingDictionary();

        object BasicTypeConverter(Type propertyType, object value);

        object LoopBackMapper(Type destinationType, Type sourceType, object sourceObj);
    }
}