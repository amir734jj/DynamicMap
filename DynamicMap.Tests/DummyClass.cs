using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicMap.Tests
{
    public class DummyClass
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBith { get; set; }

        public DummyNestedClass ParentInfo { get; set; }

        private bool Equals(DummyClass other)
        {
            return string.Equals(Name, other.Name) && Age == other.Age && DateOfBith.Equals(other.DateOfBith)
                   && Equals(ParentInfo, other.ParentInfo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((DummyClass) obj);
        }
    }

    public class DummyNestedClass
    {
        public string FatherName { get; set; }

        public string MotherName { get; set; }

        private bool Equals(DummyNestedClass other)
        {
            return string.Equals(FatherName, other.FatherName) && string.Equals(MotherName, other.MotherName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((DummyNestedClass) obj);
        }
    }

    public class DummyComplexClass
    {
        public string DateTime { get; set; }

        public DummyClass Ancesstor { get; set; }

        private bool Equals(DummyComplexClass other)
        {
            return string.Equals(DateTime, other.DateTime) && Equals(Ancesstor, other.Ancesstor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((DummyComplexClass) obj);
        }
    }

    public class DummyClassWithBasicIEnumerable
    {
        public List<string> List { get; set; }

        private bool Equals(DummyClassWithBasicIEnumerable other)
        {
            return List.Zip(other.List, string.Equals).All(x => x);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((DummyClassWithBasicIEnumerable) obj);
        }
    }
    
    public class DummyClassComplexIEnumerable
    {
        public List<DummyComplexClass> List { get; set; }

        private bool Equals(DummyClassComplexIEnumerable other)
        {
            return List.Zip(other.List, (x, y) => x.Equals(y)).All(x => x);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((DummyClassComplexIEnumerable) obj);
        }
    }
}