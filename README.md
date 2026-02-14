# Delivery Tracking API

A specialized tracking system designed for managing and monitoring deliveries in real-time. Built with .NET 8, following Domain-Driven Design (DDD) principles and CQRS with MediatR.

## 🚀 Overview

This API allows businesses to manage drivers, vehicles, and routes while tracking the progress of deliveries through a series of logged events and status updates.

### Key Features
- **Domain-Driven Design**: Core business logic encapsulated in the `Delivery` Aggregate Root.
- **Event-Driven**: Tracking is handled via a series of `DeliveryEvent` logs.
- **CQRS**: Separate models for commands (actions) and queries (reporting).
- **In-Memory Persistence**: Fast, zero-config development environment using an in-memory repository.

## 🛠️ Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Running the Application
1. Clone the repository.
2. Navigate to the project root.
3. Run the API:
   ```powershell
   dotnet run --project src\DeliveryTracking.Api
   ```
4. The API will be available at:
   - **HTTP**: `http://localhost:5000`
   - **HTTPS**: `https://localhost:5001`
5. Swagger documentation can be found at `/swagger` (e.g., `http://localhost:5000/swagger`).

## 🔄 Business Flow

The system follows a standard lifecycle for every delivery:

1.  **Preparation**: Drivers, Vehicles, and Routes are registered in the system.
2.  **Initiation**: A dispatcher creates a delivery (`POST /deliveries`), assigning a driver and vehicle to a specific route. The delivery starts in `Pending` status.
3.  **Start**: The delivery is explicitly started (`POST /deliveries/{id}/start`), moving it to `InProgress` status.
4.  **Tracking**: As the delivery progresses, the system logs events like `CheckpointReached` or `Incident` (`POST /deliveries/{id}/events`).
5.  **Completion**: Once the destination is reached, the delivery is marked as `Completed` (`POST /deliveries/{id}/complete`), freezing further modifications.
6.  **Review**: Stakeholders can retrieve a full summary and audit trail of any delivery (`GET /deliveries/{id}/summary`).

## 📡 API Reference

### Deliveries
- `POST /deliveries` - Create a new delivery (Status: Pending).
- `POST /deliveries/{id}/start` - Start a delivery (Status: InProgress).
- `POST /deliveries/{id}/events` - Log a tracking event (Checkpoint, Incident, etc.).
- `POST /deliveries/{id}/complete` - Mark a delivery as finished.
- `GET /deliveries/{id}/summary` - Get the full delivery history and status.

### Management
- `GET /drivers` | `POST /drivers` - Manage drivers.
- `GET /vehicles` | `POST /vehicles` - Manage vehicles.
- `GET /routes` | `POST /routes` - Manage routes and checkpoints.

## 🏗️ Project Structure

- **src/DeliveryTracking.Domain**: Core entities, value objects, and domain events.
- **src/DeliveryTracking.Application**: Commands, Queries, Handlers, and Interfaces.
- **src/DeliveryTracking.Infrastructure**: Persistence (In-Memory), Logging, and Dispatchers.
- **src/DeliveryTracking.Api**: Minimal API endpoints and configuration.

## 🧪 Testing

The project includes unit and integration tests.

### Integration Tests
Integration tests use `WebApplicationFactory` to test the full business flow of the API. They are organized by business flow in the [DeliveryTracking.Api.Tests.Integration](test/DeliveryTracking.Api.Tests.Integration) project:
- `HappyPathFlowTests`: Standard successful delivery lifecycle.
- `IncidentFlowTests`: Logging and tracking incidents during delivery.
- `ErrorHandlingTests`: Validation of invalid state transitions and resource lookups.

To run the tests:
```powershell
dotnet test
```

### Data Seeding

On startup, the application seeds initial data for testing:
- **Driver**: "Big Bob" (`00000000-0000-0000-0000-000000000001`)
- **Vehicle**: "Van1" (RocketVan) (`00000000-0000-0000-0000-000000000002`)
- **Route**: "ExpressTruck" (Auckland to Wellington) (`00000000-0000-0000-0000-000000000003`)

### Testing with Postman or Curl

You can interact with the API using the seeded data.

#### 1. Create a Delivery
`POST http://localhost:5000/deliveries`
```json
{
    "driverId": "00000000-0000-0000-0000-000000000001",
    "vehicleId": "00000000-0000-0000-0000-000000000002",
    "routeId": "00000000-0000-0000-0000-000000000003"
}
```
*Take note of the `id` returned in the response.*

#### 2. Start the Delivery
`POST http://localhost:5000/deliveries/{id}/start`

#### 3. Log a Tracking Event
`POST http://localhost:5000/deliveries/{id}/events`
```json
{
    "type": "CheckpointReached",
    "description": "Reached the Asteroid Belt",
    "location": "Sector 7G"
}
```
*(Event types: `RouteStarted`, `CheckpointReached`, `Incident`, `DeliveryCompleted`, `Other`)*

#### 4. Get Delivery Summary
`GET http://localhost:5000/deliveries/{id}/summary`

---

## **Future Considerations: Advancing the POC**

As this Proof of Concept evolves toward a production-grade system, several architectural enhancements should be considered to improve scalability, reliability, and security.

### **1. EF Core Integration & Unit of Work**

* **Atomic Transactions:** Use `DbContext.SaveChangesAsync()` to ensure all Aggregate Roots and related entities are persisted in a single, consistent transaction.
* **Repository Pattern:** Refactor `InMemoryRepository` to EF-backed implementations. `IDomainEventContext` can integrate with the DbContext to track aggregates automatically.

### **2. Automated Domain Event Dispatching**

* **ChangeTracker Hook:** Scan EF Core’s ChangeTracker for all `AggregateRoot` instances to register them automatically.

### **3. Reliability & the Outbox Pattern**

* **Transactional Outbox:** Persist domain events in an Outbox table within the same transaction as the business state changes to guarantee “at-least-once” delivery.
* **Background Publisher:** Use a background worker (e.g., Quartz.NET or Hangfire) to publish events from the Outbox to external systems.

### **4. Authentication & Authorization**

* **Authentication:** Ensure users (drivers, dispatchers, admins) are validated before allowing access to operations. JWT, OAuth2, or OpenID Connect are typical options.
* **Authorization:** Enforce role- or resource-based access control so users can only perform permitted actions.
* **Auditability:** Track which user performs each operation, complementing domain events and logs for accountability.
