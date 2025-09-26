using AwakenedApi.models;
using AwakenedApi.services;
using AwakenedApi.services.Interfaces;
namespace AwakenedApi.extensions;

public static class TodoRouteExtensions
{
    public static void MapTodoRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/todo").AddEndpointFilter(async (ctx, next) =>
        {
            var authService = ctx.HttpContext.RequestServices.GetRequiredService<UserAuthentication>();

            bool isAuthenticated = await authService.IsSignedInAsync(ctx.HttpContext);

            if (!isAuthenticated)
            {
                return Results.Unauthorized();
            }

            return await next(ctx);
        });

        group.MapGet("/", async (ITodoService todoService,string? userId, int pageSize = 4, int page = 1, string? search = null) =>
        {
            try
            {
                Console.WriteLine("Search: " + search);
                PagedResult<Todo> todos = await todoService.GetUserTodos(userId, page, pageSize, false, search);
                var response = new IsSuccessResponse<PagedResult<Todo>>
                    { IsSuccess = true, Message = "Success", Data = todos };
                return Results.Ok(todos); 
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });
        group.MapPost("/", async (Todo todo, ITodoService todoService, HttpContext ctx, IAIService aiService) =>
        {
            try
            {
                var userId = ctx.Items["userId"] as string;
                if (userId != todo.UserId)
                {
                    return Results.BadRequest("Invalid user id");
                }
                Todo todoWithDetails = await aiService.GetQuestDetails(todo);
                Todo createdTodo = await todoService.CreateTodo(todoWithDetails);
                return Results.Ok(createdTodo);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        group.MapPut("/", async (Todo todo, ITodoService todoService, HttpContext ctx) =>
        {
            try
            {
                var userId = ctx.Items["userId"] as string;
                if (userId != todo.UserId)
                {
                    return Results.BadRequest("Invalid user id");
                }

                await todoService.UpdateTodo(todo);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        group.MapPost("/complete",
            async (Todo todo, ITodoService todoService, IUserService userService, HttpContext ctx) =>
            {
                try
                {
                    var userId = ctx.Items["userId"] as string;
                    Console.WriteLine(userId);
                    if (userId != todo.UserId)
                    {
                        return Results.BadRequest(userId);
                    }

                    todo.Complete = true;
                    await todoService.UpdateTodo(todo);
                    await userService.UpdateStats(todo, userId);
                    Console.WriteLine("here2");
                    return Results.Ok(new IsSuccessResponse(){IsSuccess = true, Message = "Successfully Completed quest"});
                }
                catch (Exception ex)
                {
                    Console.WriteLine("here");
                    return Results.Problem(ex.Message);
                }
            });

    }
}