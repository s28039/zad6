
using System.Data.SqlClient;

namespace WebApplication1.Repositories;

public class WareHouseRepository : IWareHouseRepository
{
    private readonly IConfiguration _configuration;
    private SqlTransaction _transaction;
    private SqlConnection _connection;

    public WareHouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task BeginTransactionAsync()
    {
        _connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await _connection.OpenAsync();
        _transaction = _connection.BeginTransaction();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _connection.CloseAsync();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _connection.CloseAsync();
        }
    }

    public async Task<bool> DoesProductExist(int id)
    {
        var query = "SELECT 1 from Product where IdProduct=@ID";
        using SqlCommand command = new SqlCommand(query, _connection, _transaction);
        command.Parameters.AddWithValue("@ID", id);
        var result = await command.ExecuteScalarAsync();
        return result is not null;
    }

    public async Task<bool> DoesMagazynExist(int id)
    {
        var query = "SELECT 1 from WareHouse where idWarehouse=@ID";
        using SqlCommand command = new SqlCommand(query, _connection, _transaction);
        command.Parameters.AddWithValue("@ID", id);
        var result = await command.ExecuteScalarAsync();
        return result is not null;
    }

    public async Task<bool> InOrder(int id)
    {
        var query = "SELECT 1 FROM [Order] WHERE IdProduct = @ID";
        using SqlCommand command = new SqlCommand(query, _connection, _transaction);
        command.Parameters.AddWithValue("@ID", id);
        var result = await command.ExecuteScalarAsync();
        return result is not null;
    }

    public async Task UpdateData(int id)
    {
        var query = "UPDATE [Order] SET FulfilledAt = GETDATE() WHERE IdProduct = @ID";
        using SqlCommand command = new SqlCommand(query, _connection, _transaction);
        command.Parameters.AddWithValue("@ID", id);
        await command.ExecuteNonQueryAsync();
    }

    public async Task AddProduct(int id, int idWarehouse, int amount, string createdAt)
    {
        var query = @"
        INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)
        VALUES (@idWarehouse, @id, 1, @Amount, (SELECT price FROM Product WHERE IdProduct=@id), @CreatedAt);";
        using SqlCommand command = new SqlCommand(query, _connection, _transaction);
        command.Parameters.AddWithValue("@ID", id);
        command.Parameters.AddWithValue("@idWarehouse", idWarehouse);
        command.Parameters.AddWithValue("@Amount", amount);
        command.Parameters.AddWithValue("@CreatedAt", createdAt);
        await command.ExecuteNonQueryAsync();
    }
}
