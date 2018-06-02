using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DynamicMap.Extensions;
using Newtonsoft.Json.Linq;

namespace DynamicMap
{
    public static class DynamicMap
    {   
        /// <summary>
        /// Struct to hold on to obj's properties
        /// </summary>
        private struct PropertyInfoStruct
        {
            public string Name { get; set; }
            
            public Type PropertyType { get; set; }
            
            public PropertyInfo PropertyInfo { get; set; }
        }
        
        /// <summary>
        /// Maps an object of type object to given anonymous type with optional CustomMapping
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="customMapper"></param>
        /// <returns></returns>
        public static object Map(Type type, object obj, Dictionary<Type, Func<object, object>> customMapper = null)
        {
            customMapper = customMapper ?? new Dictionary<Type, Func<object, object>>();

            // if type is null just return null
            if (type == null) return null;
            
            // if object is null, then return default
            if (obj == null) return type.Instantiate();

            // get type of source object
            var objType = obj.GetType();

            // no mapping is needed
            if (objType == type) return obj;

            // create new instance of type
            var result = Activator.CreateInstance(type);
            
            // create properties helper
            var objTypeProperties = ToPropertyInfoStructs(obj, objType);
            
            // loop through properties
            type.GetProperties().ToDictionary(x => x, x => objTypeProperties.FirstOrDefault(y => x.Name == y.Name))
                .Where(x => x.Value.Name != null)
                .ForEach(x =>
                {
                    // if there exist a custom mapper, use that instead
                    if (customMapper.ContainsKey(x.Key.PropertyType))
                    {
                        SetValue(x.Key, result, customMapper[x.Key.PropertyType](obj));
                    }
                    else if (x.Key.PropertyType.IsIEnumerableType())
                    {
                        // instantiate IEnumerable
                        var destinationValue = (IEnumerable) x.Key.PropertyType.Instantiate();
                        
                        // safely cast value to basic IEnumerable
                        var sourceValue = (IEnumerable) GetValue(x.Key, obj, x.Value);
                        
                        // get generic type of IEnumerable of destination
                        var destinationType = x.Key.PropertyType.GetGenericType();
                        
                        // get generic type of IEnumerable of source
                        var sourceType = x.Value.PropertyType.GetGenericType();
                        
                        // safely cast value to basic IEnumerable
                        var nestedValue = (IEnumerable) x.Value.PropertyInfo.GetValue(obj);
                        
                        foreach (var nestedObj in sourceValue)
                        {
                            // ReSharper disable once PossibleMultipleEnumeration
                            AddToIEnumerable(destinationValue, Map(destinationType, nestedObj));
                        }
                        
                        // set property value
                        SetValue(x.Key, result, destinationValue);
                    }
                    else
                    {
                        // if type is complex type, then map recursively
                        SetValue(x.Key, result,
                            x.Key.PropertyType.IsSystemType()
                                ? GetValue(x.Key, obj, x.Value)
                                : Map(x.Key.PropertyType, GetValue(x.Key, obj, x.Value)));
                    }
                });

            return result;
        }

        /// <summary>
        /// Add value to IEnumerable
        /// TODO: add more IEnumerable interfaces
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="value"></param>
        private static void AddToIEnumerable(IEnumerable enumerable, object value)
        {
            switch (enumerable)
            {
                case IList list:
                    list.Add(value);
                    break;
            }
        }

        /// <summary>
        /// Converts object given it's type to IEnumerable of PropertyInfoStruct
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<PropertyInfoStruct> ToPropertyInfoStructs(object obj, Type type)
        {
            // check if object is JObject
            switch (obj)
            {
                // if type of object is JValue, use custom approach
                case JObject jObject:
                    return jObject.Properties().Select(x => new PropertyInfoStruct
                    {
                        Name = x.Name,
                        PropertyType = typeof(JValue)
                    });
                case IDictionary<string, object> keyValueDictionary:
                    return keyValueDictionary.Select(x => new PropertyInfoStruct
                    {
                        Name = x.Key,
                        PropertyType = x.Value.GetType(),
                    });
                default:
                    return type.GetProperties().Select(x => new PropertyInfoStruct
                    {
                        Name = x.Name,
                        PropertyType = x.PropertyType,
                        PropertyInfo = x
                    });
            }
        }

        /// <summary>
        /// Returns value of an object property
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="sourceObj"></param>
        /// <param name="infoStruct"></param>
        /// <returns></returns>
        private static object GetValue(MemberInfo propertyInfo, object sourceObj, PropertyInfoStruct infoStruct)
        {
            switch (sourceObj)
            {
                case IDictionary<string, object> keyValueDictionary:
                    return keyValueDictionary[propertyInfo.Name];
                case JObject jObject:
                    return jObject.GetValue(propertyInfo.Name).ToObject<object>();
                default:
                    return infoStruct.PropertyInfo.GetValue(sourceObj);
            }
        }

        /// <summary>
        /// Custom property value setter that ensures typesafety
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="sourceObj"></param>
        /// <param name="value"></param>
        private static void SetValue(PropertyInfo propertyInfo, object sourceObj, object value)
        {            
            switch (Type.GetTypeCode(propertyInfo.PropertyType))
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

            propertyInfo.SetValue(sourceObj, value);
        }

        /// <summary>
        /// Maps an object of type object to given anonymous type with optional CustomMapping and then safely cast to D
        /// </summary>
        /// <typeparam name="TD"></typeparam>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="customMapper"></param>
        /// <returns></returns>
        public static TD Map<TD>(Type type, object obj, Dictionary<Type, Func<object, object>> customMapper = null) where TD : class => Map(type, obj, customMapper) as TD;

        /// <summary>
        /// Maps an object of S object to given anonymous type with optional CustomMapping and then safely cast to D
        /// </summary>
        /// <typeparam name="TS"></typeparam>
        /// <typeparam name="TD"></typeparam>
        /// <param name="obj"></param>
        /// <param name="customMapper"></param>
        /// <returns></returns>
        public static TD Map<TS, TD>(TS obj, Dictionary<Type, Func<object, object>> customMapper = null) where TD : class => Map(typeof(TD), obj, customMapper) as TD;
    }
}