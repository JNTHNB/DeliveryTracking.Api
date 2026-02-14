using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.Interfaces;

namespace DeliveryTracking.Infrastructure.Repositories;

internal class VehicleRepository(IDomainEventContext domainEventContext)
    : InMemoryRepository<Vehicle>(domainEventContext), IVehicleRepository;