using Parking.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Parking.Service.Entities
{
    public class ParkingSpot : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(10)]
        public string Number { get; set; }

        [Required]
        public bool isAvailable { get; set; } = false;
        public string Information { get; set; }

        [ForeignKey("Park")]
        public Guid ParkingId { get; set; }

        [JsonIgnore]
        public Park Park { get; set; }

    }
}
