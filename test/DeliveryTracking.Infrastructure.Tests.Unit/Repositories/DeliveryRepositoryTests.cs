using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.Interfaces;
using DeliveryTracking.Infrastructure.Repositories;
using FluentAssertions;
using Moq;

namespace DeliveryTracking.Infrastructure.Tests.Unit.Repositories;

public class DeliveryRepositoryTests
{
    private readonly Mock<IDomainEventContext> _domainEventContextMock = new();
    private readonly DeliveryRepository _sut;

    public DeliveryRepositoryTests()
    {
        _sut = new DeliveryRepository(_domainEventContextMock.Object);
    }

    [Fact]
    public async Task GetActiveDeliveries_ShouldReturnPendingAndInProgressDeliveries()
    {
        // Arrange
        var pendingDelivery = new Delivery(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        
        var inProgressDelivery = new Delivery(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        inProgressDelivery.Start();
        
        var completedDelivery = new Delivery(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        completedDelivery.Start();
        completedDelivery.Complete();

        await _sut.Add(pendingDelivery);
        await _sut.Add(inProgressDelivery);
        await _sut.Add(completedDelivery);

        // Act
        var result = await _sut.GetActiveDeliveries();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(pendingDelivery);
        result.Should().Contain(inProgressDelivery);
        result.Should().NotContain(completedDelivery);
    }
}
