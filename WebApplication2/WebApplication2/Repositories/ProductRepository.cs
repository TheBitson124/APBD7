using Microsoft.Data.Sql;
using WebApplication2.Models;
using System;
using System.Data;
using Microsoft.Data.SqlClient;


namespace WebApplication2.Repositories;

public class ProductRepository: IProductRepository

{
    private readonly IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<bool> DoesProductExist(int IdProduct)
    {
        var  query = "Select 1 FROM Product Where IdProduct = @IdProduct ";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));

        using SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.Connection = connection;
        sqlCommand.CommandText = query;
        sqlCommand.Parameters.AddWithValue("@IdProduct", IdProduct);
        await connection.OpenAsync();
        var res = await sqlCommand.ExecuteScalarAsync();
        return res is null;
    }

    public async Task<bool> DoesWarehouseExist(int IdWarehouse)
    {
        var  query = "Select 1 FROM Warehouse Where IdWarehouse = @Id";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));

        await using SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.Connection = connection;
        sqlCommand.CommandText = query;
        sqlCommand.Parameters.AddWithValue("@Id", IdWarehouse);
        await connection.OpenAsync();
        var res = await sqlCommand.ExecuteScalarAsync();
        return res is null;
    }

    public async Task<int> DoesOrderExist(int IdProduct, int Amount)
    {
        var  query = "Select IdOrder FROM [Order] Where IdProduct = @IdProduct And Amount = @Amount ";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));

        await using SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.Connection = connection;
        sqlCommand.CommandText = query;
        sqlCommand.Parameters.AddWithValue("@IdProduct", IdProduct);
        sqlCommand.Parameters.AddWithValue("@Amount", Amount);
        await connection.OpenAsync();
        var res = await sqlCommand.ExecuteScalarAsync();
        if (res is null)
        {
            return -1;
        }

        return Convert.ToInt32(res);
    }

    public async Task<bool> DoesOrderExistInProductWarehouse(int IdOrder)
    {
        var  query = "Select 1 FROM Product_Warehouse Where IdOrder = @Id";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));

        await using SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.Connection = connection;
        sqlCommand.CommandText = query;
        sqlCommand.Parameters.AddWithValue("@Id", IdOrder);
        await connection.OpenAsync();
        var res = await sqlCommand.ExecuteScalarAsync();
        return res is null;
    }

    public async Task<bool> IsOrderFullfilled(int IdOrder)
    {
        var  query = "Select FullfilledAt FROM [Order] Where IdOrder = @IdOrder";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));

        await using SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.Connection = connection;
        sqlCommand.CommandText = query;
        sqlCommand.Parameters.AddWithValue("@IdOrder", IdOrder);
        await connection.OpenAsync();
        var res = await sqlCommand.ExecuteScalarAsync();
        return res is null;
    }
    

    public async Task<int> AddProduct(AddProductDTO data)
    {
                
        var query = "INSERT INTO Product_Warehouse VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.Connection = connection;
        sqlCommand.CommandText = query;
        sqlCommand.Parameters.AddWithValue("@IdWarehouse", data.IdWarehouse);
        sqlCommand.Parameters.AddWithValue("@IdProduct", data.IdProduct);
        sqlCommand.Parameters.AddWithValue("@IdOrder", data.IdOrder);
        sqlCommand.Parameters.AddWithValue("@Amount", data.Amount);
        sqlCommand.Parameters.AddWithValue("@Price", data.Amount * data.Price);
        sqlCommand.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
        await connection.OpenAsync();
        var res = await sqlCommand.ExecuteScalarAsync();
        
        return 1;
    }

    public async Task<bool> UpdateOrder(int IdOrder)
    {
        var  query = "Select 1 FROM Product_Warehouse Where IdOrder = @Id";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));

        await using SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.Connection = connection;
        sqlCommand.CommandText = query;
        sqlCommand.Parameters.AddWithValue("@Id", IdOrder);
        await connection.OpenAsync();
        var res = await sqlCommand.ExecuteScalarAsync();
        return res is null;
    }

    public async Task<int> GetProductPrice(int IdProduct)
    {
        var  query = "Select Price FROM Product Where IdOrder = @Id";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));

        await using SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.Connection = connection;
        sqlCommand.CommandText = query;
        sqlCommand.Parameters.AddWithValue("@Id", IdProduct);
        await connection.OpenAsync();
        var res = await sqlCommand.ExecuteScalarAsync();
        if (res is null)
        {
            return -1;
        }

        return Convert.ToInt32(res);
    }

    public async Task<int> GetMaxIdProductWarehouse()
    {
        var  query = "Select IdProductWarehouse FROM Product_Warehouse";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));

        await using SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.Connection = connection;
        sqlCommand.CommandText = query;
        await connection.OpenAsync();
        var res = await sqlCommand.ExecuteScalarAsync();
        if (res is null)
        {
            return -1;
        }
        var max = -1;
        foreach (var id in (IEnumerable<int>)res)
        {
            if (id > max)
            {
                max = id;
            }
        }
        return max;
    }
    

    public async Task<int> ExcecuteProcedure(int IdProduct,int IdWarehouse,int Amount,DateTime CreatedAt)
    {
        var query = "EXEC AddProductToWarehouse @IdProduct, @IdWarehouse, @Amount, @CreatedAt;SELECT @@IDENTITY AS NewId";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));

        await using SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.Connection = connection;
        sqlCommand.CommandText = query;
        sqlCommand.Parameters.AddWithValue("@CreatedAt", CreatedAt);
        sqlCommand.Parameters.AddWithValue("@IdProduct", IdProduct);
        sqlCommand.Parameters.AddWithValue("@IdWarehouse", IdWarehouse);
        sqlCommand.Parameters.AddWithValue("@Amount", Amount);
        await connection.OpenAsync();
        var res = await sqlCommand.ExecuteScalarAsync();
        if (res is null)
        {
            return -1;
        }
        return Convert.ToInt32(res);
    }
}