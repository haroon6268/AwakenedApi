using AwakenedApi.services.Interfaces;
namespace AwakenedApi.extensions;

public static class UserRouteExtensions
{
    public static void MapUserRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/user");
        group.MapGet("/{id}",async (string id, IUserService userService) => await userService.GetUserById((id)));
    }
}