using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DynamicMap.Builders;
using DynamicMap.Extensions;
using DynamicMap.Interfaces;
using DynamicMap.Models;

namespace DynamicMap.BaseMapper
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class BaseDynamicMap: BaseBuilder<BaseDynamicMap>, IBaseDynamicMap
    {
        protected Type _sourceType;
        protected Type _destinationType;
        protected object _sourceObj;
        protected IEnumerable<PropertyInfoStructSource> _sourcePropertyInfoStruct;
        protected IEnumerable<PropertyInfoStructDestination> _destinationPropertyInfoStruct;
        protected object _result;
        protected Dictionary<PropertyInfoStructDestination, PropertyInfoStructSource> _mappingDictionary;

        /// <summary>
        /// Create an object given destination type and do the property mappings
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="sourceType"></param>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
        public virtual object Map(Type destinationType, Type sourceType, object sourceObj)
        {
            _destinationType = destinationType;
            _sourceType = sourceType;
            _sourceObj = sourceObj;

            // If Type is primitive then use BasicTypeConverter
            if (destinationType.IsPrimitiveSystemType())
            {
                return BasicTypeConverter(destinationType, sourceObj);
            }

            _result = _destinationType.Instantiate();
            _mappingDictionary = MappingDictionary();

            _mappingDictionary.ForEach(x =>
            {
                var value = GetPropertyValue(x.Value);

                // recurisve step if type is complex type
                if (x.Value.IsComplexType)
                {
                    value = LoopBackMapper(x.Key.PropertyType, x.Value.PropertyType, value);
                }

                SetPropertyValue(x.Key, value);
            });

            return _result;
        }

        /// <summary>
        /// Convert source type to IEnumerable of PropertyInfoStruct
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<PropertyInfoStructSource> SourceToPropertyInfoStruct()
        {
            return _sourceType.GetProperties().Select(x => new PropertyInfoStructSource
            {
                Name = x.Name,
                PropertyType = x.PropertyType,
                Getter = () => x.GetValue(_sourceObj),
                IsComplexType = !x.PropertyType.IsPrimitiveSystemType()
            });
        }

        /// <summary>
        /// Convert destination type to IEnumerable of PropertyInfoStruct
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<PropertyInfoStructDestination> DestinationToPropertyInfoStruct()
        {
            return _destinationType.GetProperties().Select(x => new PropertyInfoStructDestination
            {
                Name = x.Name,
                PropertyType = x.PropertyType,
                Setter = value => x.SetValue(_result, BasicTypeConverter(x.PropertyType, value))
            });
        }

        /// <summary>
        /// Get property value
        /// </summary>
        /// <param name="infoStructSource"></param>
        /// <returns></returns>
        public object GetPropertyValue(PropertyInfoStructSource infoStructSource)
        {
            return infoStructSource.Getter();
        }
        
        /// <summary>
        /// Set property value
        /// </summary>
        /// <param name="infoStructSource"></param>
        /// <param name="value"></param>
        public virtual void SetPropertyValue(PropertyInfoStructDestination infoStructSource, object value)
        {
            infoStructSource.Setter(value);
        }

        /// <summary>
        /// Create mapping dictionary
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<PropertyInfoStructDestination, PropertyInfoStructSource> MappingDictionary()
        {
            _sourcePropertyInfoStruct = SourceToPropertyInfoStruct();
            _destinationPropertyInfoStruct = DestinationToPropertyInfoStruct();

            return _destinationPropertyInfoStruct.ToDictionary(x => x, x =>
                _sourcePropertyInfoStruct.FirstOrDefault(p => x.Name == p.Name))
                .Where(x => x.Value.Name != null)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Basic system type converter
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object BasicTypeConverter(Type propertyType, object value)
        {
            switch (Type.GetTypeCode(propertyType))
            {
                case TypeCode.Boolean:
                    value = Convert.ToBoolean(value);
                    break;
                case TypeCode.Byte:
                    value = Convert.ToByte(value);
                    break;
                case TypeCode.Char:
                    value = Convert.ToChar(value);
                    break;
                case TypeCode.DateTime:
                    value = Convert.ToDateTime(value);
                    break;
                case TypeCode.Decimal:
                    value = Convert.ToDecimal(value);
                    break;
                case TypeCode.Double:
                    value = Convert.ToDouble(value);
                    break;
                case TypeCode.Int16:
                    value = Convert.ToInt16(value);
                    break;
                case TypeCode.Int32:
                    value = Convert.ToInt32(value);
                    break;
                case TypeCode.Int64:
                    value = Convert.ToUInt64(value);
                    break;
                case TypeCode.SByte:
                    value = Convert.ToSByte(value);
                    break;
                case TypeCode.Single:
                    value = Convert.ToSingle(value);
                    break;
                case TypeCode.String:
                    value = Convert.ToString(value);
                    break;
                case TypeCode.UInt16:
                    value = Convert.ToUInt16(value);
                    break;
                case TypeCode.UInt32:
                    value = Convert.ToUInt32(value);
                    break;
                case TypeCode.UInt64:
                    value = Convert.ToUInt64(value);
                    break;
                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.DBNull:
                    break;
                default:
                    Console.WriteLine("Undefined typecode! setting value will probably fail.");
                    break;;
            }

            return value;
        }

        /// <summary>
        /// Loopback to map complex types
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="sourceType"></param>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
        public virtual object LoopBackMapper(Type destinationType, Type sourceType, object sourceObj) => DynamicMap.GetDynamicMapBuilder().RecursiveMap(destinationType, sourceType, sourceObj);
    }
}