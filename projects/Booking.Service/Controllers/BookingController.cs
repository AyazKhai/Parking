using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.Domain;
using Booking.Service.Entities;
using Parking.Domain.Dtos;
using Booking.Service.Extensions;


namespace Booking.Service.Controllers
{
    [ApiController]
    [Route("book")]
    public class BookingController : Controller
    {
        private readonly IRepository<Book> bookingRepo;

        public BookingController(IRepository<Book> bookingRepo)
        {
            this.bookingRepo = bookingRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAllAsync()
        {
            var bookings = (await bookingRepo.GetAllAsync()).Select(book => book.AsDto());
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetByIdAsync(Guid id)
        {
            var item = await bookingRepo.GetAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpGet("{userid}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetByUserIdAsync(Guid userId)
        {
            var items = (await bookingRepo.GetAllAsync())
                                         .Where(book => book.UserId == userId)
                                         .Select(book => book.AsDto());

            if (!items.Any())
            {
                return NotFound();
            }

            return Ok(items);
        }


        [HttpPost]
        public async Task<ActionResult<BookingDto>> PostAsync(CreateBookingDto book)
        {
            var newbook = new Book()
            {
                ParkingSpotId = book.ParkingSpotId,
                UserId = book.UserId,
                start = book.start,
                end = book.end,
                Status = "active"
            };

            await bookingRepo.CreateAsync(newbook);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = newbook.Id }, newbook.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateBookDto bookdto) 
        {
            var book = await bookingRepo.GetAsync(id);

            if (book is null) 
            {
                return NotFound();
            }

            book.Status = bookdto.status;

            await bookingRepo.UpdateAsync(book);
            return NoContent();
        }

    }
}
