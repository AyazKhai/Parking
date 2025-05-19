using Identity.Service.Data;
using Identity.Service.Extensions;
using Identity.Service.Models;
using Identity.Service.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.Service.Roles;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCustomAuthntication(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(RolesTypes.Admin, policy => policy.RequireRole(RolesTypes.Admin));
    options.AddPolicy(RolesTypes.Manager, policy => policy.RequireRole(RolesTypes.Manager));
    options.AddPolicy(RolesTypes.User, policy => policy.RequireRole(RolesTypes.User));
});

// Configure DBContext
builder.Services.AddDbContext<AppDBContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentityCore<User>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false; 
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDBContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

var parentDirectory = Path.GetFullPath(Path.Combine(
    builder.Environment.ContentRootPath,
    ".."  // Поднимаемся на один уровень вверх
));

// Создаём папку для ключей (например, "SharedKeys")
var keysPath = Path.Combine(parentDirectory, "SharedKeys");
Directory.CreateDirectory(keysPath);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
    .SetApplicationName("MyAuthApp");


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
//app.UseHttpsRedirection();
app.UseCors(x => x
    .WithOrigins(allowedOrigins)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.CustomMapIdentityApi<User>();
app.MapRoleManagementApi<User>();


app.Run();

