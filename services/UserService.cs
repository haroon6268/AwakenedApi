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

    public async Task<User> CreateUser(User user)
    {
        string sql = @"
            INSERT INTO users(id, username, email, strength, speed, endurance, intelligence, charisma, gold, xp, createddate, firstname, lastname)
            VALUES(@Id, @Username, @Email, @Strength, @Speed, @Endurance, @Intelligence, @Charisma, @Gold, @Xp, @Createdate, @Firstname, @Lastname)";
        var parameters = new
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Strength = user.Strength ?? 0,
            Speed = user.Speed ?? 0,
            Endurance = user.Endurance ?? 0,
            Intelligence = user.Intelligence ?? 0,
            Charisma = user.Charisma ?? 0,
            Gold = user.Gold ?? 0,
            Xp = user.Xp ?? 0,
            Createdate = DateTimeOffset.UtcNow,
            Firstname = user.Firstname,
            Lastname = user.Lastname
        };
        await _connection.ExecuteAsync(sql, parameters);
        return user;
    }
    
    
}