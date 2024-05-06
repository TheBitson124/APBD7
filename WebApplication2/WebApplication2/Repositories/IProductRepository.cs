using WebApplication2.Models;

namespace WebApplication2.Repositories;

public interface IProductRepository
{
    Task<bool> DoesProductExist(int IdProduct);
    Task<bool> DoesWarehouseExist(int IdWarehouse);
    Task<bool> DoesOrderExist(int IdProduct, int Amount);
    Task<bool> IsOrderFullfilled(int IdOrder);
    Task<int> AddProduct(WarehouseDataDTO data);
}