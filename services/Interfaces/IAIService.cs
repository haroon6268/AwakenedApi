using AwakenedApi.models;

namespace AwakenedApi.services.Interfaces
{
    public interface IAIService
    {
        public Task<string> sendChat(string message);
        public Task<Todo> GetQuestDetails(Todo todo);
        public Task<List<Todo>> GetQuestRecommendations(List<Todo> todo);
    }


}
