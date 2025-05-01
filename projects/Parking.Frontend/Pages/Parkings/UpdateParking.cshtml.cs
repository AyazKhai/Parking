using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parking.Frontend.Dtos;
using Parking.Frontend.Services;

namespace Parking.Frontend.Pages.Parkings
{
        public class UpdateParkingModel : PageModel
        {
            [BindProperty]
            public ParkDto updatePark { get; set; }
            private readonly IParkingApiService _parkingService;

            public UpdateParkingModel(IParkingApiService parkingService)
            {
                _parkingService = parkingService;
            }

            [TempData]
            public string SuccessMessage { get; set; }

            [TempData]
            public string ErrorMessage { get; set; }
             public async Task<IActionResult> OnGetAsync(Guid id)
            {
                try
                {
                    updatePark = await _parkingService.GetByIdAsync(id);
                    if (updatePark == null)
                    {
                        return NotFound();
                    }

                    return Page();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Ошибка при загрузке данных: {ex.Message}";
                    return RedirectToPage("/Parkings/Parkings");
                }
            }

            public async Task<IActionResult> OnPostAsync() 
            {
                if (!ModelState.IsValid || updatePark == null)
                {
                    return Page();
                }
                try
                {
                    await _parkingService.UpdateAsync(updatePark.id, new UpdateParkDto(updatePark.address, updatePark.information, updatePark.isAvailable));
                    SuccessMessage = "Парковка успешно обновлена!";
                    return RedirectToPage("/Parkings/Parkings");
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Ошибка при обновлении парковки: {ex.Message}";
                    return Page();
                }
            }
    }
}
