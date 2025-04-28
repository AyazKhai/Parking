namespace Parking.Service.Contracts;
public record ParkCreatedContract(Guid id, string address, bool isAvailable);
public record ParkUpdatedContract(Guid id, string address, bool isAvailable);
public record ParkDeleteContract(Guid id);

public record ParkingSpotCreatedContract(Guid id, string number,string information, bool isAvailable);
public record ParkingSpotUpdatedContract(Guid id, string number, string information, bool isAvailable);
public record ParkingSpotDeleteContract(Guid id);

