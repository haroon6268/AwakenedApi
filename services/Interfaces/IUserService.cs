using AwakenedApi.models;
namespace AwakenedApi.services.Interfaces;

public interface IUserService
{
    public Task<User?> GetUserById(string id);
    public Task<UpdateStatsResponse> UpdateStats(Todo todo, string userId);
    public Task<User> CreateUser(User user);
}