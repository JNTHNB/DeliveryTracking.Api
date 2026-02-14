using DeliveryTracking.Application.Behaviors;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.Interfaces;
using DeliveryTracking.Domain.Services;
using MediatR;

namespace DeliveryTracking.Application.Tests.Unit.Behaviors;

public class DomainEventDispatchBehaviorTests
{
    private readonly Mock<IDomainEventDispatcher> _dispatcherMock = new();
    private readonly IDomainEventContext _context = new DomainEventContext();

    private record TestRequest : IRequest<bool>;

    [Fact]
    public async Task Handle_ShouldDispatchEvents_FromRegisteredAggregates()
    {
        // Arrange
        var behavior = new DomainEventDispatchBehavior<TestRequest, bool>(_context, _dispatcherMock.Object);
        var aggregate = new Delivery(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        aggregate.Start(); // Adds a Domain Event
        
        _context.RegisterAggregate(aggregate);

        // Act
        await behavior.Handle(new TestRequest(), _ => Task.FromResult(true), CancellationToken.None);

        // Assert
        _dispatcherMock.Verify(d => d.Dispatch(It.IsAny<IDomainEvent>()), Times.Once);
        aggregate.DomainEvents.Should().BeEmpty();
        _context.GetRegisteredAggregates().Should().BeEmpty();
    }
}
