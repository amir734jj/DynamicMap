using System;
using System.Collections.Generic;

namespace DynamicMap.Tests.Models
{
    public class EnumerableModel
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBith { get; set; }

        public FlatModel ParentInfo { get; set; }
        
        public List<FlatModel> Anccesstors { get; set; }
        
        public List<int> CountOfAnccesstors { get; set; }
    }
}