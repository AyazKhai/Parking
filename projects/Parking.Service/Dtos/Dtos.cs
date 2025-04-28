
using Parking.Service.Entities;
using System.ComponentModel.DataAnnotations;

namespace Parking.API.Dtos
{
    public record ParkDto(Guid id, [Required] string address, string information, bool isAvailable, ICollection<ParkingSpot> ParkingSpots);
    public record CreateParkDto([Required] string address, bool isAvailable, string information);
    public record UpdateParkDto([Required] string address, string information, bool isAvailable);

    public record ParkingSpotDto(Guid id, string number,bool isAvailable, string information, Guid ParkingID);
    public record CreateParkingSpotDto([Required]string number, bool isAvailable, string information, [Required]Guid ParkingID);
    public record UpdateParkingSpotDto([Required] string Number, bool isAvailable, string information);
}
