using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Data;
using System.Security.Claims;

public static class RoleManagementEndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapRoleManagementApi<TUser>(
        this IEndpointRouteBuilder endpoints)
        where TUser : class
    {
        var routeGroup = endpoints.MapGroup("/admin/roles")
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        // Назначить роль пользователю
        routeGroup.MapPost("/assign", async Task<Results<Ok, NotFound<string>, BadRequest<string>, ValidationProblem>>
            ([FromBody] AssignRoleRequest request, [FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<UserManager<TUser>>();
            var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return TypedResults.NotFound("User not found");

            if (!await roleManager.RoleExistsAsync(request.RoleName))
                return TypedResults.BadRequest("Role does not exist");

            // Удаляем все текущие роли
            var currentRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, currentRoles);

            // Назначаем новую роль
            var result = await userManager.AddToRoleAsync(user, request.RoleName);

            return result.Succeeded ?
                TypedResults.Ok() :
                CreateValidationProblem(result);
        });

        routeGroup.MapGet("/users", async Task<Ok<List<UserWithRolesDto>>>
            ([FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<UserManager<TUser>>();
            var users = userManager.Users.ToList();

            var result = new List<UserWithRolesDto>();
            foreach (var user in users)
            {
                result.Add(new UserWithRolesDto
                {
                    Email = await userManager.GetEmailAsync(user),
                    Roles = await userManager.GetRolesAsync(user)
                });
            }

            return TypedResults.Ok(result);
        });

        return new RoleManagementEndpointsConventionBuilder(routeGroup);
    }

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        var errorDictionary = new Dictionary<string, string[]>();

        foreach (var error in result.Errors)
        {
            errorDictionary.Add(error.Code, new[] { error.Description });
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }

    private sealed class RoleManagementEndpointsConventionBuilder(RouteGroupBuilder inner)
        : IEndpointConventionBuilder
    {
        private IEndpointConventionBuilder InnerAsConventionBuilder => inner;

        public void Add(Action<EndpointBuilder> convention) =>
            InnerAsConventionBuilder.Add(convention);

        public void Finally(Action<EndpointBuilder> finallyConvention) =>
            InnerAsConventionBuilder.Finally(finallyConvention);
    }
}

public class AssignRoleRequest
{
    public required string Email { get; set; }
    public required string RoleName { get; set; }
}

public class UserWithRolesDto
{
    public required string Email { get; set; }
    public required IList<string> Roles { get; set; }
}