using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Parking.Domain.PostgreDb
{
    public static  class Extensions
    {
        public static IServiceCollection AddPostgres<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                
                );

            return services;
        }

        public static IServiceCollection AddEfRepository<TEntity, TContext>(this IServiceCollection services)
        where TEntity : class, IEntity
        where TContext : DbContext
        {
            services.AddScoped<IRepository<TEntity>>(provider =>
            {
                var context = provider.GetRequiredService<TContext>();
                return new EfRepository<TEntity>(context);
            });

            return services;
        }
    }
}
