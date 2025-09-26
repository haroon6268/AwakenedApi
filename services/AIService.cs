using AwakenedApi.models;
using AwakenedApi.services.Interfaces;
using Npgsql;
using OpenAI.Chat;
using System.Text.Json;

namespace AwakenedApi.services
{
    public class AIService : IAIService
    {
        private readonly ChatClient _chatClient;
        private readonly ITodoService _todoService;

        public AIService(ChatClient chatClient)
        {
            _chatClient = chatClient;
        }
        public async Task<string> sendChat(string message)
        {
            ChatCompletion completion = await _chatClient.CompleteChatAsync(message);
            return completion.Content[0].Text;
        }

        public async Task<Todo> GetQuestDetails(Todo todo)
        {
            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "quest_details",
                    jsonSchema: BinaryData.FromBytes("""
                        {
                            "type": "object",
                            "properties": {
                                "xp":{
                                    "type":"integer",
                                    "description": "The amount of XP this quest should receive"
                                 },
                                "gold":{
                                    "type":"integer",
                                    "description": "The amount of gold this quest should receive"
                                },
                                "rank":{
                                    "type":"integer",
                                    "description":"The rank of this quest. This ranges from 0-4. 0 being the lowest and 4 being the hardest rank"
                                }
                             },
                           "required":["gold","xp", "rank"],
                           "additionalProperties": false
                        }
                """u8.ToArray()),
                    jsonSchemaIsStrict: true
                )
            };

            List<ChatMessage> messages = [
                 new UserChatMessage($@"
                You are a game master. Decide XP, Gold amount, **and a Rank (0-4)** for quests,
                where Rank 0 is easiest and Rank 4 is hardest.

                Example Quests:
                    1. Quest: Wash Dishes | XP: 10 | Gold: 5 | Rank: 0
                    2. Quest: Clean Apartment | XP: 40 | Gold: 20 | Rank: 1
                    3. Quest: 5 Mile Run | XP: 80 | Gold: 40 | Rank: 3

                New Quest:
                Quest: {todo.Name}
                Description: {todo.Description}
             ")
                ];
            ChatCompletion completion = _chatClient.CompleteChat(messages, options);
            using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);
            int gold = structuredJson.RootElement.GetProperty("gold").GetInt32();
            int xp = structuredJson.RootElement.GetProperty("xp").GetInt32();
            int rank = structuredJson.RootElement.GetProperty("rank").GetInt32();
            todo.Gold = gold;
            todo.Xp = xp;
            if(rank < 0 || rank > 4)
            {
                rank = 2;
            }
            todo.Rank = (Rank)rank;
            return todo;

        }

        public async Task<List<Todo>> GetQuestRecommendations(List<Todo> todo)
        {
            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
    jsonSchemaFormatName: "quest_recommendations",
    jsonSchema: BinaryData.FromBytes("""
    {
        "type": "object",
        "properties": {
            "quests": {
                "type": "array",
                "items": {
                    "type": "object",
                     "additionalProperties": false,
                    "properties": {
                        "name": { "type": "string" },
                        "description": { "type": "string" },
                    
                    },
                    "required": ["name", "description"]
                }
            }
        },
        "required": ["quests"],
        "additionalProperties": false   
    }
    """u8.ToArray()), jsonSchemaIsStrict: true)
            };

            List<ChatMessage> messages = new()
{
    new UserChatMessage(
        "You are a game master. The next message will contain a JSON array listing the quests "
      + "the player has already completed. Based on that list, respond with EXACTLY three new "
      + "quests that will help the player progress. Each quest must include only the fields "
      + "`name` and `description`, matching the provided JSON schema."    ),
    new UserChatMessage(JsonSerializer.Serialize(todo))
};
            ChatCompletion completion = _chatClient.CompleteChat(messages, options);
            return new List<Todo>();     
                }

    }
}
