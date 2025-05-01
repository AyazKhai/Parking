using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parking.Frontend.Dtos;
using Parking.Frontend.Services;
using System.ComponentModel.DataAnnotations;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Parking.Frontend.Pages.Parkings
{
    
    public class ParkingsModel : PageModel
    {
        private readonly IParkingApiService _parkingService;
        public List<ParkDto> parks { get; set; } = new();
        [TempData]
        public string SuccessMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public ParkingsModel(IParkingApiService parkingService)
        {
            _parkingService = parkingService;
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id) 
        {
            if (!ModelState.IsValid) { return Page(); }

            try
            {
                await _parkingService.DeleteAsync(id);
                SuccessMessage = "Парковка успешно удалена!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при удалении парковки: {ex.Message}";
            }
            return RedirectToPage();


        }

        public async Task OnGet()
        {
            try
            {
                parks = await _parkingService.GetAllAsync(); 
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ErrorMessage = "Сервис парковок не найден";
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = $"Ошибка HTTP: {ex.StatusCode}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Произошла ошибка: {ex.Message}";
            }
        }
    }
}
