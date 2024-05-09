using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public WarehouseController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpPost("normal")]
    public async Task<IActionResult> PostProduct(HttpDTO data)
    {
        if (!await _productRepository.DoesProductExist(data.IdProduct))
        {
            return NotFound($"no prodcut with Id {data.IdProduct}");
        }
        if (!await _productRepository.DoesWarehouseExist(data.IdWarehouse))
        {
            return NotFound($"no Warehouse with Id {data.IdWarehouse}");
        }

        var IdOrder = await _productRepository.DoesOrderExist(data.IdProduct,data.Amount);
        if (IdOrder == -1)
        {   
            return NotFound($"no Order with Id {data.IdProduct} and Amount {data.Amount}");
        }
        if (await _productRepository.DoesOrderExistInProductWarehouse(IdOrder))
        {
            return NotFound("juz zrobione zamówienie");
        }
        var addProduct = new AddProductDTO();
        addProduct.IdProduct = data.IdProduct;
        addProduct.IdProductWarehouse = await _productRepository.GetMaxIdProductWarehouse();
        addProduct.IdWarehouse = data.IdWarehouse;
        addProduct.Amount = data.Amount;
        addProduct.IdOrder = IdOrder;
        addProduct.Timestamp = DateTime.Now;
        addProduct.Price = await _productRepository.GetProductPrice(data.IdProduct) * data.Amount;
       
        using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _productRepository.UpdateOrder(IdOrder);
            await  _productRepository.AddProduct(addProduct);
            scope.Complete();
        }
        return Ok();
    }
    [HttpPost("Procedure")]
    public async Task<ActionResult> AddProductToWarehouse(HttpDTO data)
    {
        using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var res = await _productRepository.ExcecuteProcedure(data.IdProduct, data.IdWarehouse, data.Amount,
                data.CreatedAt);
            scope.Complete();
            if (res == -1)
            {
                return NotFound("doesnt work");
            }

            return Ok(res);
        }
    }
}