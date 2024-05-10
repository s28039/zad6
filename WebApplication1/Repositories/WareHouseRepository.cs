using System.Data.SqlClient;

namespace WebApplication1.Repositories;

public class WareHouseRepository : IWareHouseRepository
{
    private static int ile = 0;
    private readonly IConfiguration _configuration;
    public WareHouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    
    public async Task<bool> DoesProductExist(int id)
    {
        var query = "SELECT 1 from Product where IdProduct=@ID";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var x = await command.ExecuteScalarAsync();
        
        return x is not null;
    }

    public async Task<bool> DoesMagazynExist(int id)
    {
        var query = "SELECT 1 from WareHouse where idWarehouse=@ID";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var x = await command.ExecuteScalarAsync();
        
        return x is not null;
    }
    
    public async Task<bool> InOrder(int id)
    {
        var query = "SELECT 1 FROM [Order] WHERE IdProduct = @ID";
        
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var X = await command.ExecuteScalarAsync();

        return X is not null;
    }
    public async Task<bool> IsFulfilledAt(int id)
    {
        var query = "SELECT 1 FROM Product_Warehouse WHERE IdOrder = @ID";
        
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var X = await command.ExecuteScalarAsync();

        return X is not null;
    }
    
    public async Task UpdateData(int id)
    {
        var query = "UPDATE [Order] SET FulfilledAt = GETDATE() WHERE IdProduct = @ID";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }

    public Task<int> InsertData(int id)
    {
        throw new NotImplementedException();
    }
    public Task ReturnKey(int id)
    {
        return ReturnKey(id);
    }

    public async Task AddProduct(int id, int idWarehouse, int Amount, string CreatedAt)
    {
        
        
        
        var query = @"INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)
        VALUES (@idWarehouse, @id, 1,@Amount, (select price from product where IdProduct=@id), @CreatedAt );";
        ile += 1;
        
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);
        command.Parameters.AddWithValue("@idWarehouse", idWarehouse);
        command.Parameters.AddWithValue("@Amount", Amount);
        command.Parameters.AddWithValue("@CreatedAt", CreatedAt);
        
        
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        
        
        
        query = "SELECT SCOPE_IDENTITY()";
        command.CommandText = query;
        
    }
    
}