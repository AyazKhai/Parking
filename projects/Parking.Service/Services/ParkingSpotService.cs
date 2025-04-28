using Microsoft.EntityFrameworkCore;
using Parking.API.Data;
using Parking.Domain;
using Parking.Service.Entities;
using System.Linq;
using System.Linq.Expressions;

namespace Parking.API.Services
{
    public class ParkingSpotService : IRepository<ParkingSpot>
    {
        private readonly ApplcationDbContext _context;

        public ParkingSpotService(ApplcationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(ParkingSpot entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.ParkingSpots.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<ParkingSpot>> GetAllAsync()
        {
            return await _context.ParkingSpots.ToListAsync();
        }

        public async Task<ICollection<ParkingSpot>> GetAllAsync(Expression<Func<ParkingSpot, bool>> filter)
        {
            return await _context.ParkingSpots
                .Where(filter)
                .ToListAsync();
        }

        public async Task<ParkingSpot> GetAsync(Guid id)
        {
            return await _context.ParkingSpots
              .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<ParkingSpot> GetAsync(Expression<Func<ParkingSpot, bool>> filter, Guid id)
        {
            return await _context.ParkingSpots
                .Where(filter).
                FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await GetAsync(id);
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _context.ParkingSpots.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ParkingSpot entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _context.ParkingSpots.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
