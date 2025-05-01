using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Parking.API.Data;
using Microsoft.EntityFrameworkCore.Design;
using Parking.Domain;
using Parking.API.Services;
using Parking.Domain.PostgreDb;
using Parking.Service.Entities;
using Parking.Domain.MassTransit;


namespace Parking.API
{
    public class Startup
    {
        private const string AllowedOriginSettings = "AllowedOrigin";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPostgres<ApplcationDbContext>(Configuration)
                    .AddEfRepository<Park, ApplcationDbContext>()
                    .AddEfRepository<ParkingSpot, ApplcationDbContext>()
                    .AddMassTransittWithRabbitMq(); ;

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;// to stop deleting suffix Async in methods for corenctly web working
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Parking.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parking.API v1"));

                app.UseCors(builder =>
                {
                    builder.WithOrigins("AllowedOrigin")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(x => 
            {
                x.WithHeaders().AllowAnyHeader();
                x.WithOrigins("http://localhost:3000");
                x.WithMethods().AllowAnyMethod();
            });
        }
    }
}
