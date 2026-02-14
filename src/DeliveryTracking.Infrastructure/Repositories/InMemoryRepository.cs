using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.Interfaces;
using System.Collections.Concurrent;

namespace DeliveryTracking.Infrastructure.Repositories;

public abstract class InMemoryRepository<T>(IDomainEventContext? domainEventContext = null)
    : IRepository<T> where T : class
{
    protected static readonly ConcurrentDictionary<Guid, T> Entities = new();

    public Task<T?> Find(Guid id)
    {
        Entities.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task<IEnumerable<T>> List()
    {
        return Task.FromResult<IEnumerable<T>>(Entities.Values);
    }

    public Task Add(T entity)
    {
        var idProperty = typeof(T).GetProperty("Id");
        if (idProperty == null) throw new Exception("Entity does not have an Id property");
        var id = (Guid)idProperty.GetValue(entity)!;
        Entities[id] = entity;

        if (entity is AggregateRoot aggregate)
        {
            domainEventContext?.RegisterAggregate(aggregate);
        }

        return Task.CompletedTask;
    }

    public Task Update(T entity)
    {
        var idProperty = typeof(T).GetProperty("Id");
        if (idProperty == null) throw new Exception("Entity does not have an Id property");
        var id = (Guid)idProperty.GetValue(entity)!;
        Entities[id] = entity;

        if (entity is AggregateRoot aggregate)
        {
            domainEventContext?.RegisterAggregate(aggregate);
        }

        return Task.CompletedTask;
    }
}