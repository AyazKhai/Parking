using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Parking.Domain;

namespace Booking.Service.Entities;

public class ParkEntity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Address { get; set; }

    public bool IsAvailable { get; set; } = false;

}
