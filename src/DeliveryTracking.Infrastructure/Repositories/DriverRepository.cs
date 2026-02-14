using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.Interfaces;

namespace DeliveryTracking.Infrastructure.Repositories;

internal class DriverRepository(IDomainEventContext domainEventContext) : InMemoryRepository<Driver>(domainEventContext), IDriverRepository;

