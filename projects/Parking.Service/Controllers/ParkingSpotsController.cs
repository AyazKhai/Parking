using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.API.Dtos;
using Parking.Domain;
using Parking.Service.Attributes;
using Parking.Service.Contracts;
using Parking.Service.Entities;

namespace Parking.API.Controllers
{
    [ApiController]
    [Route("parkingSpots")]
    [Authorize]
    public class ParkingSpotsController : ControllerBase
    {
        private readonly IRepository<ParkingSpot> parkingsSpotRepos;
        private readonly IPublishEndpoint publishEndpoints;

        public ParkingSpotsController(IRepository<ParkingSpot> parkingsSpotRepos, IPublishEndpoint publishEndpoints)
        {
            this.parkingsSpotRepos = parkingsSpotRepos;
            this.publishEndpoints = publishEndpoints;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ParkingSpotDto>>> GetAsync()
        {
            var rnd = new Random();
            var spots = (await parkingsSpotRepos.GetAllAsync()).Select(spots => spots.AsDto());
            return Ok(spots);
        }

        [HttpGet("{id}")]
        [EnsureParkingSpotExists]
        [AllowAnonymous]
        public async Task<ActionResult<ParkingSpotDto>> GetByIdAsync(Guid id) 
        {
            var item = await parkingsSpotRepos.GetAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrManager")]
        public async Task<ActionResult<ParkingSpotDto>> PostAsync(CreateParkingSpotDto crSpot) 
        {
            var spot = new ParkingSpot()
            {
                Number = crSpot.number,
                isAvailable = crSpot.isAvailable,
                Information = crSpot.information,
                ParkingId = crSpot.ParkingID
            };

            await parkingsSpotRepos.CreateAsync(spot);
            await publishEndpoints.Publish(new ParkingSpotCreatedContract(spot.Id, spot.Number, spot.Information, spot.isAvailable));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = spot.Id }, spot.AsDto());
        }

        [HttpPut]
        [Authorize(Policy = "AdminOrManager")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateParkingSpotDto upSpot) 
        {
            var spot = await parkingsSpotRepos.GetAsync(id);
            if (spot is null)
            {
                var newSpot = new ParkingSpot()
                {
                    Number = upSpot.Number,
                    isAvailable = upSpot.isAvailable,
                    Information = upSpot.information
                };
                await parkingsSpotRepos.CreateAsync(newSpot);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = newSpot.Id }, newSpot.AsDto());
            }
            spot.Number = upSpot.Number;
            spot.isAvailable = upSpot.isAvailable;
            spot.Information = upSpot.information;

            await parkingsSpotRepos.UpdateAsync(spot);
            await publishEndpoints.Publish(new ParkingSpotUpdatedContract(spot.Id, spot.Number, spot.Information, spot.isAvailable));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        [EnsureParkingSpotExists]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await parkingsSpotRepos.GetAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            await parkingsSpotRepos.RemoveAsync(item.Id);
            await publishEndpoints.Publish(new ParkingSpotDeleteContract(item.Id));
            return NoContent();
        }
    }

}
