using AwakenedApi.models;
using AwakenedApi.services;
using AwakenedApi.services.Interfaces;

namespace AwakenedApi.extensions;

public static class ShopItemRouteExtensions
{
    public static void MapShopItemRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/shopitem").AddEndpointFilter(async (ctx, next) =>
        {
            var authService = ctx.HttpContext.RequestServices.GetRequiredService<UserAuthentication>();

            bool isAuthenticated = await authService.IsSignedInAsync(ctx.HttpContext);

            if (!isAuthenticated)
            {
                return Results.Unauthorized();
            }

            return await next(ctx);
        });

        group.MapGet("/", async (IShopItemService shopItemService) =>
        {
            try
            {
                List<ShopItem> shopItems = await shopItemService.GetAllShopItems();
                var response = new IsSuccessResponse<List<ShopItem>>
                    { IsSuccess = true, Message = "Success", Data = shopItems };
                return Results.Ok(shopItems);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });
    }
}
