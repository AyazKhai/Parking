using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Parking.API.Dtos;
using Parking.Domain;
using Parking.Service.Contracts;
using Parking.Service.Entities;

namespace Parking.API.Controllers
{
    [ApiController]
    [Route("parking")]
    public class ParkingController : ControllerBase
    {
        private readonly IRepository<Park> ParkingRepos;
        private readonly IPublishEndpoint publishEndpoints;

        public ParkingController(IRepository<Park> Repos, IPublishEndpoint publishEndpoints)
        {
            ParkingRepos = Repos;
            this.publishEndpoints = publishEndpoints;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkDto>>> GetAsync()
        {
            var parks = (await ParkingRepos.GetAllAsync()).Select(Park => Park.AsDto());
            return Ok(parks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkDto>> GetByIdAsync(Guid id) 
        {
            var item = await ParkingRepos.GetAsync(id);
            if (item is null) 
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ParkDto>> PostAsync(CreateParkDto crPark) 
        {
            var park = new Park()
            {
                Address = crPark.address,
                Information = crPark.information,
                IsAvailable = crPark.isAvailable   
            };

            await ParkingRepos.CreateAsync(park);
            await publishEndpoints.Publish(new ParkCreatedContract(park.Id, park.Address, park.IsAvailable));
            return CreatedAtAction(nameof(GetByIdAsync), new { id = park.Id }, park.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateParkDto upadtedParkDto) 
        {
            var park = await ParkingRepos.GetAsync(id);
            if (park is null)
            {
                var newpark = new Park()
                {
                    Address = upadtedParkDto.address,
                    Information = upadtedParkDto.information
                };
                await ParkingRepos.CreateAsync(newpark);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = newpark.Id }, newpark.AsDto());
            }

            park.Address = upadtedParkDto.address;
            park.Information = upadtedParkDto.information;
            park.IsAvailable = upadtedParkDto.isAvailable;

            await ParkingRepos.UpdateAsync(park);
            await publishEndpoints.Publish(new ParkUpdatedContract(park.Id, park.Address, park.IsAvailable));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await ParkingRepos.GetAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            await ParkingRepos.RemoveAsync(item.Id);
            await publishEndpoints.Publish(new ParkDeleteContract(item.Id));

            return NoContent();
        }
    }
}
