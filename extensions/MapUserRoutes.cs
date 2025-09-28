using AwakenedApi.services.Interfaces;
using AwakenedApi.models;
namespace AwakenedApi.extensions;

public static class UserRouteExtensions
{
    public static void MapUserRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/user");
        group.MapGet("/{id}",async (string id, IUserService userService) => await userService.GetUserById((id)));
        group.MapPost("/", async (User user, IUserService userService) => await userService.CreateUser(user));
    }
}