namespace WebApplication2.Models;

public class AddProductDTO
{
    public int IdProductWarehouse { get; set; }
    public int IdWarehouse { get; set; }
    public int IdProduct { get; set; }
    public int IdOrder{ get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
    public DateTime Timestamp { get; set; }
}