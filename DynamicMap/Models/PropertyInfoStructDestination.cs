using System;

namespace DynamicMap.Models
{
    /// <summary>
    /// Struct to hold on to obj's properties for destination
    /// </summary>
    public struct PropertyInfoStructDestination
    {
        public string Name { get; set; }
            
        public Type PropertyType { get; set; }
        
        public Action<object> Setter { get; set; }
    }
}