using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MMLib.Ocelot.Provider.AppConfiguration;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotGateway.Extensions;
using System.Net.Http.Headers;
using System.Text;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCustomAuthntication(builder.Configuration);


var parentDirectory = Path.GetFullPath(Path.Combine(
    builder.Environment.ContentRootPath,
    ".."  // Поднимаемся на один уровень вверх
));

// Создаём папку для ключей (например, "SharedKeys")
var keysPath = Path.Combine(parentDirectory, "SharedKeys");
Directory.CreateDirectory(keysPath); 

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
    .SetApplicationName("MyAuthApp")
    .SetDefaultKeyLifetime(TimeSpan.FromDays(15))
    .AddKeyManagementOptions(options =>
    {
        options.NewKeyLifetime = TimeSpan.FromDays(15);
        options.AutoGenerateKeys = true;
    });


#region Ocelot
builder.Configuration.AddOcelotWithSwaggerSupport(options =>
{
    options.Folder = "OcelotConfig";
});

builder.Services.AddOcelot(builder.Configuration)
    .AddAppConfiguration();
builder.Services.AddSwaggerForOcelot(builder.Configuration);
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseRouting();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None,
    Secure = CookieSecurePolicy.Always,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always
});

app.UseAuthentication();
app.UseAuthorization(); 


app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
}).UseWebSockets();

app.MapGet("/", () => "API Gateway is running");

await app.UseOcelot(); 

app.Run();
