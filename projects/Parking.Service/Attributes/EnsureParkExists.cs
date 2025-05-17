using Microsoft.AspNetCore.Mvc;
using Parking.Domain.Filters;
using Parking.Service.Entities;

namespace Parking.Service.Attributes
{
    public class EnsureParkExists : TypeFilterAttribute
    {
        public EnsureParkExists() : base(typeof(EnsureEntityExistsFilter<Park>)) { }
    }
}
