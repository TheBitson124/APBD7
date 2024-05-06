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

    [HttpPost]
    public IActionResult PostProduct(WarehouseDataDTO data)
    {
        var key = _productRepository.AddProduct(data);
        return Ok(key);
    }
}