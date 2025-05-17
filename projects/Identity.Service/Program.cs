using Identity.Service.Data;
using Identity.Service.Extensions;
using Identity.Service.Models;
using Identity.Service.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.Service.Roles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme; 
    options.DefaultChallengeScheme = IdentityConstants.BearerScheme; 
}).AddCookie(IdentityConstants.ApplicationScheme).AddBearerToken(IdentityConstants.BearerScheme);


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(RolesTypes.Admin, policy => policy.RequireRole(RolesTypes.Admin));
    options.AddPolicy(RolesTypes.Manager, policy => policy.RequireRole(RolesTypes.Manager));
    options.AddPolicy(RolesTypes.User, policy => policy.RequireRole(RolesTypes.User));
});

// Configure DBContext
builder.Services.AddDbContext<AppDBContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add IdentityCore
builder.Services
    .AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDBContext>()
    .AddApiEndpoints(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.CustomMapIdentityApi<User>();
app.MapRoleManagementApi<User>();
//app.MapIdentityApi<User>();
app.Run();

