using System.Collections;

namespace DynamicMap.Tests.Utilities
{
    public class AssertExtension
    {
        protected static void Assert(object expected, object obj, IEqualityComparer comparer)
        {
            Xunit.Assert.True(comparer.Equals(expected, obj));
        }
    }
}