using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Domain.Dtos
{
    public record BookingDto(Guid ParkingSpotId, Guid UserID, string status, DateTimeOffset start, DateTimeOffset end);
    public record CreateBookingDto(Guid ParkingSpotId, Guid UserId, DateTimeOffset start, DateTimeOffset end);

    public record UpdateBookDto(string status);

}
