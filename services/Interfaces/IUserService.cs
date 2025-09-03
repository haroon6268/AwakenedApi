using AwakenedApi.models;
namespace AwakenedApi.services.Interfaces;

public interface IUserService
{
    public Task<User> GetUserById(string id);
    public Task UpdateStats(Todo todo, string userId);
}