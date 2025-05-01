using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parking.Frontend.Dtos;
using Parking.Frontend.Services;

namespace Parking.Frontend.Pages.Parkings
{
    public class CreateParkingModel : PageModel
    {
        [BindProperty]
        public CreateParkDto createPark { get; set; }
        private readonly IParkingApiService _parkingService;
        [TempData]
        public string SuccessMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public CreateParkingModel(IParkingApiService parkingService)
        {
            _parkingService = parkingService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _parkingService.CreateAsync(createPark);
                SuccessMessage = "Парковка успешно добавлена!";
                return RedirectToPage(); 
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при добавлении парковки: {ex.Message}";
                return Page();
            }
        }

    }
}
