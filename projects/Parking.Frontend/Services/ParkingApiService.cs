using Microsoft.AspNetCore.Mvc;
using Parking.Frontend.Dtos;
using System.Net;

namespace Parking.Frontend.Services
{
    public class ParkingApiService : IParkingApiService
    {
        private readonly HttpClient _httpClient;

        public ParkingApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ParkDto>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/parking");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<ParkDto>>() ?? new();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Ошибка при получении списка парковок", ex);
            }
        }

        public async Task<ParkDto?> GetByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/parking/{id}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ParkDto>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка при получении парковки с ID {id}", ex);
            }
        }

        public async Task<ParkDto> CreateAsync(CreateParkDto park)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/parking", park);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ParkDto>()
                    ?? throw new Exception("Не удалось десериализовать ответ");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Ошибка при создании парковки", ex);
            }
        }

        public async Task UpdateAsync(Guid id, UpdateParkDto park)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/parking/{id}", park);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка при обновлении парковки с ID {id}", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/parking/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка при удалении парковки с ID {id}", ex);
            }
        }
    }
}
