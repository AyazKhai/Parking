using Microsoft.AspNetCore.Mvc;
using Parking.Domain.Filters;
using Parking.Service.Entities;

namespace Parking.Service.Attributes
{
    public class EnsureParkingSpotExists : TypeFilterAttribute
    {
        public EnsureParkingSpotExists() : base(typeof(EnsureEntityExistsFilter<ParkingSpot>)) { }
    }
}
