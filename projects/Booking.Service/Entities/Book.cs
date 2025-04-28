using Parking.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Booking.Service.Entities
{
    public class Book : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        [Required]
        public Guid ParkingSpotId { get; set; }

        [Required]
        public string Status { get; set; }
        [Required]
        public DateTimeOffset start { get; init; }
        [Required]
        public DateTimeOffset end { get; init; }
        

        [JsonIgnore]
        public ParkingSpotEntity parkingSpot {get; set;}

        
    }
}
