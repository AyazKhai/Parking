using Booking.Service.Entities;
using MassTransit;
using Parking.Domain;
using Parking.Service.Contracts;

namespace Booking.Service.Consumers
{
    public class ParkCreatedConsumer : IConsumer<ParkCreatedContract>
    {
        private readonly IRepository<ParkEntity> repository;

        public ParkCreatedConsumer(IRepository<ParkEntity> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<ParkCreatedContract> context)
        {
            var message = context.Message;

            var park = await repository.GetAsync(message.id);

            if (park != null)
            {
                return;
            }

            park = new ParkEntity
            {
                Id = message.id,
                Address = message.address,
                IsAvailable = message.isAvailable,
            };

            await repository.CreateAsync(park);
        }
    }
}
