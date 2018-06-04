namespace DynamicMap.Interfaces
{
    public interface IBaseBuilder<out T>
    {
        T New();
    }
}