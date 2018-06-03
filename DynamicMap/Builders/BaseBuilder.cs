namespace DynamicMap.Builders
{
    /// <summary>
    /// Base builder
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseBuilder<T> where T : new()
    {
        public T New() => new T();
    }
}