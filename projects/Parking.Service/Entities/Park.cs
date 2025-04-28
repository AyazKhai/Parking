using Parking.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Service.Entities
{
    public class Park : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Address { get; set; }

        public bool IsAvailable { get; set; } = false;
        public string Information { get; set; }
        public ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();

    }
}
