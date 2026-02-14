using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.Interfaces;

namespace DeliveryTracking.Infrastructure.Repositories;

internal class RouteRepository(IDomainEventContext domainEventContext) : InMemoryRepository<Route>(domainEventContext), IRouteRepository;

