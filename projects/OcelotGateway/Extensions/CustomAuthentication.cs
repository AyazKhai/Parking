using Microsoft.AspNetCore.Identity;

namespace OcelotGateway.Extensions
{
    public static class CustomAuthentication
    {
        public static IServiceCollection AddCustomAuthntication(this IServiceCollection services, IConfiguration configuration)
        {
            var cookieSettings = configuration.GetSection("CookieAuthentication");
            services.AddAuthentication(IdentityConstants.ApplicationScheme)
                    .AddCookie("Identity.Application", options =>
                    {
                        options.Cookie.Name = cookieSettings["Name"];
                        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        options.Cookie.Domain = cookieSettings["Domain"];
                    });

            return services;
        }
    }
}
