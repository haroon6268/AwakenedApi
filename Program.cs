using AwakenedApi.extensions;
using AwakenedApi.models;
using AwakenedApi.services;
using AwakenedApi.services.Interfaces;
using Npgsql;
using OpenAI.Chat;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetValue<string>("ConnectionString");
    return new NpgsqlConnection(connectionString);
});
builder.Services.AddHealthChecks();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserAuthentication>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IShopItemService, ShopItemService>();
builder.Services.AddSingleton<ChatClient>(sp =>
{
    var apiKey = sp.GetRequiredService<IConfiguration>().GetValue<string>("OPEN_AI_KEY");
    var model = "gpt-5-nano";
    return new ChatClient(model, apiKey);
});
builder.Services.AddScoped<IAIService, AIService>();
var app = builder.Build();

app.MapGet("/", async (HttpContext ctx, UserAuthentication service) => await service.IsSignedInAsync(ctx));
app.MapGet("/AITest", async (IAIService _aiService, ITodoService _todoService) =>
{
    PagedResult<Todo> paged = await _todoService.GetUserTodos("user_31Omg1dOZYIcGis2E3saI4Q4Y9H");
    List<Todo> todos = paged.Items;
    await _aiService.GetQuestRecommendations(todos);
});
app.MapUserRoutes();
app.MapTodoRoutes();
app.MapShopItemRoutes();
app.MapHealthChecks("/health");
app.Run();
