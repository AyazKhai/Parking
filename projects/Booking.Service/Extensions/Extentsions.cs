using Booking.Service.Entities;
using Parking.Domain.Dtos;
using System.Runtime.CompilerServices;

namespace Booking.Service.Extensions
{
    public static class Extentsions
    {
        public static BookingDto AsDto(this Book book) 
        {
            return new BookingDto(book.ParkingSpotId, book.UserId, book.Status, book.start, book.end);
        }
    }
}
