using Parking.Frontend.Dtos;

namespace Parking.Frontend.Services
{
    public interface IParkingApiService
    {
        Task<ParkDto> CreateAsync(CreateParkDto park);
        Task DeleteAsync(Guid id);
        Task<List<ParkDto>> GetAllAsync();
        Task<ParkDto?> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, UpdateParkDto park);
    }
}