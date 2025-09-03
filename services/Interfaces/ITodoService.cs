using AwakenedApi.models;

namespace AwakenedApi.services.Interfaces;

public interface ITodoService
{
    public Task CreateTodo(Todo todo);
    public Task<PagedResult<Todo>> GetUserTodos(string? userId, int page = 0, int pageSize = 2, bool? complete = false, string? search = null);

    public Task UpdateTodo(Todo todo);
}