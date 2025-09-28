using AwakenedApi.models;

namespace AwakenedApi.services.Interfaces;

public interface IShopItemService
{
    public Task<List<ShopItem>> GetAllShopItems();
}
