
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IWareHouseRepository _warehouseRepository;

    public WarehouseController(IWareHouseRepository wareHouseRepository)
    {
        _warehouseRepository = wareHouseRepository;
    }
    
    [HttpPost("AddProduct/{productId}")]
    public async Task<IActionResult> AddProductToWarehouse(int productId, int idWarehouse, int amount, string createdAt)
    {
        if (!await _warehouseRepository.DoesMagazynExist(idWarehouse) || !await _warehouseRepository.DoesProductExist(productId) || !await _warehouseRepository.IsFulfilledAt(idWarehouse))
        {
            return NotFound();
        }
        
        try
        {
            await _warehouseRepository.BeginTransactionAsync();
            
            if (!await _warehouseRepository.InOrder(productId))
            {
                await _warehouseRepository.UpdateData(productId);
            }
            
            await _warehouseRepository.AddProduct(productId, idWarehouse, amount, createdAt);
            
            await _warehouseRepository.CommitTransactionAsync();
            
            return Ok();
        }
        catch (Exception)
        {
            await _warehouseRepository.RollbackTransactionAsync();
            return StatusCode(500, "Internal server error");
        }
    }
}
