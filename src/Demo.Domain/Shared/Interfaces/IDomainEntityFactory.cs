namespace Demo.Domain.Shared.Interfaces
{
    public interface IDomainEntityFactory
    {
        T CreateInstance<T>();
    }
}
