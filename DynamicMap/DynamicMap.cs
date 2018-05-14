using System;
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

            if (type == null) return null;

            // create new instance of type
            var result = Activator.CreateInstance(type);
            
            // if object is null, then return default
            if (obj == null) return result;
            
            var objType = obj.GetType();
            IEnumerable<PropertyInfoStruct> objTypeProperties;
            
            // check if object is JObject
            if (objType == typeof(JObject))
            {
                objTypeProperties = ((JObject) obj).Properties().Select(x => new PropertyInfoStruct
                {
                    Name = x.Name,
                    PropertyType = typeof(JValue)
                });
            }
            else
            {
                objTypeProperties = objType.GetProperties().Select(x => new PropertyInfoStruct
                {
                    Name = x.Name,
                    PropertyType = x.PropertyType
                });
            }

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
                    // if type of object is JValue, use custom approach
                    else if (x.Value.PropertyType == typeof(JValue))
                    {
                        SetValue(x.Key, result, (obj as JObject)?.GetValue(x.Key.Name).ToObject<object>());                        
                    }
                    // if type of object is System type, then go with a simple get/set value
                    else if (x.Key.PropertyType.IsSystemType())
                    {
                        SetValue(x.Key, result, objType.GetProperty(x.Key.Name).GetValue(obj));
                    }
                    // if object is complex type then do a recursive step
                    else
                    {
                        SetValue(x.Key, result, Map(x.Key.PropertyType, objType.GetProperty(x.Key.Name).GetValue(obj)));
                    }
                });

            return result;
        }

        /// <summary>
        /// Custom property value setter that ensures typesafety
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        private static void SetValue(PropertyInfo propertyInfo, object obj, object value)
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

            propertyInfo.SetValue(obj, value);
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