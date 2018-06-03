using System;

namespace DynamicMap.Tests.Models
{
    public class ComplexModel
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBith { get; set; }

        public FlatModel ParentInfo { get; set; }

        private bool Equals(ComplexModel other)
        {
            return string.Equals(Name, other.Name) && Age == other.Age && DateOfBith.Equals(other.DateOfBith) && Equals(ParentInfo, other.ParentInfo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((ComplexModel) obj);
        }
    }
}