using Npgsql;
using AwakenedApi.models;
using AwakenedApi.services.Interfaces;
using Dapper;

namespace AwakenedApi.services;

public class UserService : IUserService
{
    private readonly NpgsqlConnection _connection;
    
    public UserService(NpgsqlConnection sqlConnection)
    {
        _connection = sqlConnection;
    }

    public async Task<User?> GetUserById(string userId)
    {
        string sql = @"select * from users where id = @userId";
        var parameters = new { userId = userId };
        User? result = await _connection.QuerySingleOrDefaultAsync<User?>(sql, parameters);
        return result;
    }

    public async Task UpdateUser(User user)
    {
        string sql =
            $"UPDATE users SET username = ${user.Username}, strength=${user.Strength}, speed=${user.Speed}, endurance=${user.Endurance}, intelligence=${user.Intelligence}, charisma=${user.Charisma}, gold=${user.Gold}, xp=${user.Xp}, firstname=${user.Firstname}, lastname=${user.Lastname} WHERE id=${user.Id}";
            await _connection.ExecuteAsync(sql);
    }

    public async Task UpdateStats(Todo todo, string userId)
    {
        int xp = todo.Xp ?? 0;
        int gold = todo.Gold ?? 0;

        string sql = "UPDATE users SET xp = xp + @xp, gold = gold + @gold WHERE id = @id";
        var parameters = new {xp=xp,gold=gold,id=userId};
        await _connection.ExecuteAsync(sql, parameters);

    }
    
    
}