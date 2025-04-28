using Booking.Service.Entities;

namespace Booking.Service.Client
{
    public class ParkingClient
    {
        private readonly HttpClient httpClient;

        public ParkingClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ICollection<ParkEntity>> GetParksEntityAsync()
        {
            var parks = await httpClient.GetFromJsonAsync<ICollection<ParkEntity>>("/parking");
            return parks;
        }
    }
}
