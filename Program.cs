using AwakenedApi.extensions;
using AwakenedApi.services;
using AwakenedApi.services.Interfaces;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetValue<string>("ConnectionString");
    return new NpgsqlConnection(connectionString);
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserAuthentication>();
builder.Services.AddScoped<ITodoService, TodoService>();
var app = builder.Build();

app.MapGet("/", async (HttpContext ctx, UserAuthentication service) => await service.IsSignedInAsync(ctx));
app.MapUserRoutes();
app.MapTodoRoutes();
app.Run();
