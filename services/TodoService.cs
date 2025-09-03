using AwakenedApi.models;
using AwakenedApi.services.Interfaces;
using Dapper;
using Npgsql;

namespace AwakenedApi.services;

public class TodoService : ITodoService
{
    private readonly NpgsqlConnection _connection;

    public TodoService(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    
    public async Task CreateTodo(Todo todo)
    {
        string sql = @"
            INSERT INTO todoitems(id, name, description, rank, xp, gold, userId, createddate, duedate)
            VALUES(@id, @name, @description, @rank, @xp, @gold, @userId, now(), @duedate);
        ";

        var parameters = new
        {
            id = todo.Id, name = todo.Name, description = todo.Description, rank = todo.Rank, xp = todo.Xp,
            gold = todo.Xp, userId = todo.UserId, duedate = todo.Duedate
        };

        await _connection.ExecuteAsync(sql, parameters);
    }

   
    public async Task<PagedResult<Todo>> GetUserTodos(string userId, int page = 1, int pageSize = 2, bool? complete = null, string? search = null)
    {
        int offset = (page - 1) * pageSize;
        string searchToUse = search;
        // Base query for filtering
        var sqlBase = @"
        FROM todoitems
        WHERE userId = @userId
    ";
        
        if (complete.HasValue)
            sqlBase += " AND complete = @complete";
        if (search != null)
        {
            sqlBase += " AND name ILIKE @search";
            searchToUse = $"%{searchToUse}%";
        }

        // Query for total count
        var countSql = "SELECT COUNT(*) " + sqlBase;
        var totalCount = await _connection.ExecuteScalarAsync<int>(countSql, new { userId, complete, search = searchToUse});

        // Query for paginated results
        var dataSql = "SELECT * " + sqlBase + " ORDER BY duedate ";
        var todos = await _connection.QueryAsync<Todo>(dataSql, new { userId, pageSize, offset, complete, search = searchToUse });

        return new PagedResult<Todo>
        {
            Items = todos.ToList(),
            TotalCount = totalCount,
            Pager = new Pager(){TotalPages = (int)Math.Ceiling((double)totalCount/pageSize), Page = page, PageSize = pageSize, Search = search}
        };
    }

    public async Task UpdateTodo(Todo todo)
    {
        string sql = @"UPDATE todoitems 
               SET name = @name, 
                   description = @description, 
                   rank = @rank, 
                   xp = @xp, 
                   gold = @gold, 
                   duedate = @duedate, 
                   complete = @complete  
               WHERE id = @id";
        var parameters = new
        {
            name = todo.Name, description = todo.Description, rank = todo.Rank, xp = todo.Xp, gold = todo.Gold,
            duedate = todo.Duedate, complete = todo.Complete, id = todo.Id
        };
        await _connection.ExecuteAsync(sql, parameters);
    }
}