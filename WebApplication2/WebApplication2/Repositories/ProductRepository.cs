using WebApplication2.Models;

namespace WebApplication2.Repositories;

public class ProductRepository: IProductRepository

{
    public async Task<bool> DoesProductExist(int IdProduct)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DoesWarehouseExist(int IdWarehouse)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DoesOrderExist(int IdProduct, int Amount)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsOrderFullfilled(int IdOrder)
    {
        throw new NotImplementedException();
    }

    public async Task<int> AddProduct(WarehouseDataDTO data)
    {
        return 0;
    }
}