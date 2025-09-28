using AwakenedApi.models;
using AwakenedApi.services.Interfaces;
using Dapper;
using Npgsql;

namespace AwakenedApi.services;

public class ShopItemService : IShopItemService
{
    private readonly NpgsqlConnection _connection;

    public ShopItemService(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<List<ShopItem>> GetAllShopItems()
    {
        string sql = @"
            SELECT name, description, price
            FROM shopitem
            ORDER BY name
        ";

        var shopItems = await _connection.QueryAsync<ShopItem>(sql);
        return shopItems.ToList();
    }
}
