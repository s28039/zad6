namespace WebApplication1.Repositories;

public interface IWareHouseRepository
{
    Task<bool> DoesProductExist(int id);
    Task<bool> DoesMagazynExist(int id);
    Task<bool> IsFulfilledAt(int id);
    Task<bool> InOrder(int id);
    Task UpdateData(int id);
    Task AddProduct(int id, int idWarehouse, int amount, string createdAt);
 
}