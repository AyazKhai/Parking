namespace Identity.Service.Extensions
{
    public static class CustomAuthentication
    {
        public static IServiceCollection AddCustomAuthntication(this IServiceCollection services, IConfiguration configuration)
        {
            var cookieSettings = configuration.GetSection("CookieAuthentication");
            services.AddAuthentication()
                    .AddCookie("Identity.Application", options =>
                    {
                        options.Cookie.Name = cookieSettings["Name"];
                        options.Cookie.SameSite = SameSiteMode.None;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        options.ExpireTimeSpan = TimeSpan.FromDays(15);
                        options.Cookie.Domain = cookieSettings["Domain"];
                    });
            return services;
        }
    }
}
