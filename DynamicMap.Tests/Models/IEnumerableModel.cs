using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DynamicMap.Tests.Models
{
    public class EnumerableModelSource: ComplexModelSource
    {        
        public List<FlatModelSource> Anccesstors { get; set; }
        
        public List<int> CountOfAnccesstors { get; set; }

        protected bool Equals(EnumerableModelSource other)
        {
            return base.Equals(other) && Equals(Anccesstors, other.Anccesstors) && Equals(CountOfAnccesstors, other.CountOfAnccesstors);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EnumerableModelSource) obj);
        }
    }
    
    public class EnumerableModelDestination: ComplexModelDestination
    {        
        public List<FlatModelDestination> Anccesstors { get; set; }
        
        public List<int> CountOfAnccesstors { get; set; }

        protected bool Equals(EnumerableModelDestination other)
        {
            return base.Equals(other) && Equals(Anccesstors, other.Anccesstors) && Equals(CountOfAnccesstors, other.CountOfAnccesstors);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EnumerableModelDestination) obj);
        }
    }

    public class EnumerableModelComparer : IEqualityComparer
    {
        public bool Equals(object x, object y)
        {
            if (x is EnumerableModelSource source && y is EnumerableModelDestination destination)
            {
                var complexComparer = new ComplexModelComparer();

                var flatComparer = new FlatModelComparer();

                return complexComparer.Equals(x, y)
                       && source.CountOfAnccesstors.Zip(destination.CountOfAnccesstors, (i, j) => i == j).All(b => b)
                       && source.Anccesstors.Zip(destination.Anccesstors, (i, j) => flatComparer.Equals(i, j)).All(b => b);
            }

            throw new NotImplementedException();
        }

        public int GetHashCode(object obj) => throw new NotImplementedException();
    }
}