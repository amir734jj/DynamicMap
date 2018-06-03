using System;

namespace DynamicMap.Tests.Models
{
    public class FlatModel
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBith { get; set; }

        private bool Equals(FlatModel other)
        {
            return string.Equals(Name, other.Name) && Age == other.Age && DateOfBith.Equals(other.DateOfBith);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((FlatModel) obj);
        }
    }
}