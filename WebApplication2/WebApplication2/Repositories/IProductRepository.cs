using WebApplication2.Models;

namespace WebApplication2.Repositories;

public interface IProductRepository
{
    Task<bool> DoesProductExist(int IdProduct);
    Task<bool> DoesWarehouseExist(int IdWarehouse);
    Task<int> DoesOrderExist(int IdProduct, int Amount);
    Task<bool> DoesOrderExistInProductWarehouse(int IdOrder);
    Task<bool> IsOrderFullfilled(int IdOrder);
    Task<int> AddProduct(AddProductDTO data);
    Task<bool> UpdateOrder(int IdOrder);
    Task<int> GetProductPrice(int IdProduct);
    Task<int> GetMaxIdProductWarehouse();
    Task<int> ExcecuteProcedure(int IdProduct,int IdWarehouse,int Amount,DateTime CreatedAt);
}