using System;
using System.Reflection;

namespace DynamicMap.Models
{
    /// <summary>
    /// Struct to hold on to obj's properties for source
    /// </summary>
    public struct PropertyInfoStructSource
    {
        public string Name { get; set; }
            
        public Type PropertyType { get; set; }
           
        public Func<object> Getter { get; set; }
        
        public bool IsComplexType { get; set; }
    }
}