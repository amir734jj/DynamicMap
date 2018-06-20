using System.Collections.Generic;

namespace DynamicMap.Utilities
{
    public class ParamsWrapper<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _seq;

        public ParamsWrapper(IEnumerable<T> seq)
        {
            _seq = seq;
        }

        public static implicit operator ParamsWrapper<T>(T instance)
        {
            return new ParamsWrapper<T>(new[] { instance });
        }

        public static implicit operator ParamsWrapper<T>(List<T> seq)
        {
            return new ParamsWrapper<T>(seq);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _seq.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}