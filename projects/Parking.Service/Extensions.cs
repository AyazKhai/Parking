using Parking.API.Dtos;
using Parking.Service.Entities;

namespace Parking.API
{
    public static class Extensions
    {
        public static ParkDto AsDto(this Park park) 
        {
            return new ParkDto(park.Id, park.Address, park.Information,  park.IsAvailable, park.ParkingSpots);
        }

        public static ParkingSpotDto AsDto(this ParkingSpot spot) 
        {
            return new ParkingSpotDto(spot.Id, spot.Number, spot.isAvailable, spot.Information, spot.ParkingId);
        }
    }
}
