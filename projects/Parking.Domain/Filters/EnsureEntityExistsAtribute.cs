using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Domain.Filters
{
    public class EnsureEntityExistsFilter<T> : IAsyncActionFilter where T : class, IEntity
    {
        private readonly IRepository<T> _service;

        public EnsureEntityExistsFilter(IRepository<T> service)
        {
            _service = service;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.TryGetValue("id", out var idParam))
            {
                context.Result = new BadRequestResult();
                return;
            }

            if (!Guid.TryParse(idParam.ToString(), out Guid guid))
            {
                context.Result = new BadRequestObjectResult("Invalid ID format. Expected GUID");
                return;
            }

            var entity = await _service.GetAsync(guid);

            if (entity == null)
            {
                context.Result = new NotFoundObjectResult($"Entity with ID not found");
                return;
            }

            await next();
        }

    }
}
