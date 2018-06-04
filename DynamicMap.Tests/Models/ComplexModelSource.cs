using System;
using System.Collections;

namespace DynamicMap.Tests.Models
{
    public class ComplexModelSource: FlatModelSource
    {
        public FlatModelSource ParentInfo { get; set; }

        protected bool Equals(ComplexModelSource other)
        {
            return base.Equals(other) && Equals(ParentInfo, other.ParentInfo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ComplexModelSource) obj);
        }
    }
    
    public class ComplexModelDestination: FlatModelDestination
    {
        public FlatModelDestination ParentInfo { get; set; }

        protected bool Equals(ComplexModelDestination other)
        {
            return base.Equals(other) && Equals(ParentInfo, other.ParentInfo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ComplexModelDestination) obj);
        }
    }
    
    public class ComplexModelComparer : IEqualityComparer
    {
        public new bool Equals(object x, object y)
        {
            if (x is ComplexModelSource source && y is ComplexModelDestination destination)
            {
                var comparer = new FlatModelComparer();

                return comparer.Equals(x, y) && comparer.Equals(source.ParentInfo, destination.ParentInfo);
            }

            throw new NotImplementedException();
        }

        public int GetHashCode(object obj) => throw new NotImplementedException();
    }
}