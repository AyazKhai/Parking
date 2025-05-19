using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Parking.API.Data;
using Microsoft.EntityFrameworkCore.Design;
using Parking.Domain;
using Parking.API.Services;
using Parking.Domain.PostgreDb;
using Parking.Service.Entities;
using Parking.Domain.MassTransit;
using Parking.Domain.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;


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
            services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo("C:/temp/keys/")) 
                    .SetApplicationName("MyAuthApp");

            services.AddAuthentication(IdentityConstants.ApplicationScheme)
                    .AddCookie("Identity.Application", options =>
                    {
                        options.Cookie.Name = Configuration["CookieAuthentication:Name"];
                        options.Cookie.SameSite = SameSiteMode.None;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        options.ExpireTimeSpan = TimeSpan.FromDays(15);
                        options.Cookie.Domain = Configuration["CookieAuthentication:Domain"];
                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Manager", policy => policy.RequireRole("Manager"));
                options.AddPolicy("User", policy => policy.RequireRole("User"));
                options.AddPolicy("AdminOrManager", policy => policy.RequireRole("Admin", "Manager"));

                options.FallbackPolicy = null;
            });

            services.AddPostgres<ApplcationDbContext>(Configuration)
                    .AddEfRepository<Park, ApplcationDbContext>()
                    .AddEfRepository<ParkingSpot, ApplcationDbContext>()
                    .AddMassTransittWithRabbitMq();

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
            }

            app.UseRouting();

            var allowedOrigins = Configuration.GetSection("AllowedOrigins").Get<string[]>();

            app.UseCors(x => x
                .WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None,
                Secure = CookieSecurePolicy.Always,
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
