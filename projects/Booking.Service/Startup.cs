using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Booking.Service.Data;
using Parking.Domain.PostgreDb;
using Booking.Service.Entities;
using Parking.Domain.MassTransit;
using Booking.Service.Client;
using Polly;
using Polly.Timeout;
using Parking.Domain.Settings;

namespace Booking.Service
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
                    .AddEfRepository<Book, ApplcationDbContext>()
                    .AddEfRepository<ParkEntity, ApplcationDbContext>()
                    .AddEfRepository<ParkingSpotEntity, ApplcationDbContext>()
                    .AddMassTransittWithRabbitMq();

            AddParkingClient(services);

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;// to stop deleting suffix Async in methods for corenctly web working
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking.API v1"));

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
        }
        private static void AddParkingClient(IServiceCollection services)
        {
            Random jitterer = new Random();

            services.AddHttpClient<ParkingClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001");
            })
            .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
                5,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)),//по экспоненте
                onRetry: (outcome, timespan, retryAttempt) =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    serviceProvider.GetService<ILogger<ParkingClient>>()?
                        .LogWarning($"Delaying for {timespan.TotalSeconds} seconds, then making retry {retryAttempt}");
                }
            ))
            .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(//автоматический выключатель 
                3,
                TimeSpan.FromSeconds(15),
                onBreak: (outcome, timespan) =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    serviceProvider.GetService<ILogger<ParkingClient>>()?
                        .LogWarning($"Opening the circuit for {timespan.TotalSeconds} seconds...");
                },
                onReset: () =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    serviceProvider.GetService<ILogger<ParkingClient>>()?
                        .LogWarning($"Closing the circuit...");
                }
            ))
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));//при вызове 5001 ждет 1 секунду выполннения прежде чем гив апп
        }
    }


    
}
