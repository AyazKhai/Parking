using Booking.Service.Entities;
using MassTransit;
using Parking.Domain;
using Parking.Domain.PostgreDb;
using Parking.Service.Contracts;

namespace Booking.Service.Consumers
{
    public class ParkUpdatedConsumer : IConsumer<ParkUpdatedContract>
    {
        private readonly IRepository<ParkEntity> _repository;

        public ParkUpdatedConsumer(IRepository<ParkEntity> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ParkUpdatedContract> context)
        {
            var message = context.Message;
            var park = await _repository.GetAsync(message.id);

            if (park != null) 
            {
                park.Address = message.address;
                park.IsAvailable = message.isAvailable;

                await _repository.UpdateAsync(park);
            }
        }
    }
}
