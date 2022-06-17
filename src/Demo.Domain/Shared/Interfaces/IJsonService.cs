namespace Demo.Domain.Shared.Interfaces
{
    public interface IJsonService<T>
    {
        string ToJson(T entity);
        T FromJson(string json);
    }
}