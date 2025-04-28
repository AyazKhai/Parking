using Parking.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Booking.Service.Entities
{
    public class ParkingSpotEntity: IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(10)]
        public string Number { get; set; }

        public string Information { get; set; }
        [Required]
        public bool isAvailable { get; set; } = false;

        [ForeignKey("Park")]
        public Guid ParkingId { get; set; }

    }
}
