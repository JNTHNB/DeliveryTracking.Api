namespace DeliveryTracking.Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> Find(Guid id);
    Task<IEnumerable<T>> List();
    Task Add(T entity);
    Task Update(T entity);
}