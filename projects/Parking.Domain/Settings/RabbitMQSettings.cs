using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Domain.Settings
{
    public class RabbitMQSettings
    {
        public required string Host { get; init; }
    }
}
