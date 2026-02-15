namespace DeliveryTracking.Domain.Exceptions;

public abstract class DomainException(string message) : Exception(message);

public class DeliveryAlreadyStartedException() : DomainException("Delivery has already been started.");

public class DeliveryNotInProgressException() : DomainException("Delivery is not in progress.");

public class DeliveryAlreadyFinishedException() : DomainException("Cannot perform this action on a finished delivery.");
