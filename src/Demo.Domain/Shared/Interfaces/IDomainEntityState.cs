namespace Demo.Domain.Shared.Interfaces
{
    public interface IDomainEntityState
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);
        bool TryGet<T>(string key, out T value);
    }
}
