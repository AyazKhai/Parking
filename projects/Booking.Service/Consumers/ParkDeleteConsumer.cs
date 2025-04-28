using Booking.Service.Entities;
using MassTransit;
using Parking.Domain;
using Parking.Service.Contracts;

namespace Booking.Service.Consumers
{
    public class ParkDeleteConsumer : IConsumer<ParkDeleteContract>
    {
        private readonly IRepository<ParkEntity> repository;

        public ParkDeleteConsumer(IRepository<ParkEntity> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<ParkDeleteContract> context)
        {
            var message = context.Message;

            var park = await repository.GetAsync(message.id);

            if (park == null) { return; }

            await repository.RemoveAsync(message.id);
        }
    }
}
