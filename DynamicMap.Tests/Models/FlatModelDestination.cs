using System;
using System.Collections;

namespace DynamicMap.Tests.Models
{
    public class FlatModelSource
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBith { get; set; }

        protected bool Equals(FlatModelSource other)
        {
            return string.Equals(Name, other.Name) && Age == other.Age && DateOfBith.Equals(other.DateOfBith);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FlatModelSource) obj);
        }
    }
    
    public class FlatModelDestination
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBith { get; set; }

        protected bool Equals(FlatModelDestination other)
        {
            return string.Equals(Name, other.Name) && Age == other.Age && DateOfBith.Equals(other.DateOfBith);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FlatModelDestination) obj);
        }
    }

    public class FlatModelComparer : IEqualityComparer
    {
        public new bool Equals(object x, object y)
        {
            if (x is FlatModelSource source && y is FlatModelDestination destination)
            {
                return source.Name == destination.Name && source.Age == destination.Age
                                                       && source.DateOfBith == destination.DateOfBith;
            }

            throw new NotImplementedException();
        }

        public int GetHashCode(object obj) => throw new NotImplementedException();
    }
}