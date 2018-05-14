using System;

namespace DynamicMap.Tests
{
    public class DummyClass
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBith { get; set; }

        public DummyNestedClass ParentInfo { get; set; }

        // public DummyComplexClass MoreInfo { get; set; }

        private bool Equals(DummyClass other)
        {
            return string.Equals(Name, other.Name) && Age == other.Age && DateOfBith.Equals(other.DateOfBith)
                   && Equals(ParentInfo, other.ParentInfo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DummyClass) obj);
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
            return obj.GetType() == this.GetType() && Equals((DummyNestedClass) obj);
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
            return obj.GetType() == this.GetType() && Equals((DummyComplexClass) obj);
        }
    }
}