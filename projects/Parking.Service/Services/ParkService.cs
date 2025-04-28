using Microsoft.EntityFrameworkCore;
using Parking.API.Data;
using Parking.Domain;
using Parking.Service.Entities;
using System.Linq.Expressions;

namespace Parking.API.Services
{
    public class ParkService : IRepository<Park>
    {
        private readonly ApplcationDbContext _context;

        public ParkService(ApplcationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Park entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Parks.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Park>> GetAllAsync()
        {
            return await _context.Parks.Include(p => p.ParkingSpots).ToListAsync();
        }

        public async Task<ICollection<Park>> GetAllAsync(Expression<Func<Park, bool>> filter)
        {
            return await _context.Parks
                .Include(p => p.ParkingSpots)
                .Where(filter)
                .ToListAsync();
        }

        public async Task<Park> GetAsync(Guid id)
        {
            return await _context.Parks
               .Include(p => p.ParkingSpots)
               .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Park> GetAsync(System.Linq.Expressions.Expression<Func<Park, bool>> filter, Guid id)
        {
            return await _context.Parks
                .Where(filter).
                FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await GetAsync(id);
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _context.Parks.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Park entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _context.Parks.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
