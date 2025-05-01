using System.ComponentModel.DataAnnotations;

namespace Parking.Frontend.Dtos
{
    public record ParkDto(Guid id, [Required] string address, string information, bool isAvailable);//ICollection<ParkingSpot> ParkingSpots)
    public record CreateParkDto([Required] string address, bool isAvailable, string information);
    public record UpdateParkDto([Required] string address, string information, bool isAvailable);
}
