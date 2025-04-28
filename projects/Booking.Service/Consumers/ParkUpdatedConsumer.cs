using Booking.Service.Entities;
using MassTransit;
using Parking.Domain;
using Parking.Service.Contracts;

namespace Booking.Service.Consumers
{
    public class ParkUpdatedConsumer : IConsumer<ParkUpdatedContract>
    {
        private readonly IRepository<ParkEntity> repository;

        public ParkUpdatedConsumer(IRepository<ParkEntity> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<ParkUpdatedContract> context)
        {
            var message = context.Message;
            var park = await repository.GetAsync(message.id);

            if (park != null) 
            {
                park = new ParkEntity
                {
                    Id = message.id,
                    Address = message.address,
                    IsAvailable = message.isAvailable
                };

                await repository.UpdateAsync(park);
            }
        }
    }
}
