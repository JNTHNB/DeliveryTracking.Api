namespace DeliveryTracking.Application.Exceptions;

public class IdNotFoundException(string name, object id) : Exception($"{name} Id {id} invalid");