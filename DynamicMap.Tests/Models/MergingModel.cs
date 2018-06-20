using System;
using System.Collections;

namespace DynamicMap.Tests.Models
{
    public class MergingModel
    {
        public string StrProp1 { get; set; }
        
        public string StrProp2 { get; set; }
        
        public string StrProp3 { get; set; }
      
        public DateTime? DateTimeProp1 { get; set; }
        
        public DateTime? DateTimeProp2 { get; set; }
        
        public DateTime? DateTimeProp3 { get; set; }
    }
    
    public class MergingModelComparer : IEqualityComparer
    {
        public new bool Equals(object x, object y)
        {
            if (x is MergingModel source && y is MergingModel destination)
            {
                return source.StrProp1 == destination.StrProp1
                       && source.StrProp2 == destination.StrProp2
                       && source.StrProp3 == destination.StrProp3
                       && source.DateTimeProp1 == destination.DateTimeProp1
                       && source.DateTimeProp2 == destination.DateTimeProp2
                       && source.DateTimeProp3 == destination.DateTimeProp3;
            }

            throw new NotImplementedException();
        }

        public int GetHashCode(object obj) => throw new NotImplementedException();
    }
}