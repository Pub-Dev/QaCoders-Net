using Dapper;
using QaCoders_Net.Entities;
using QaCoders_Net.Interfaces.Repositories;
using System.Data;

namespace QaCoders_Net.Repositories;

public class OrderProductRepository : IOrderProductRepository
{
    private readonly IDbConnection _dbConnection;

    public OrderProductRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task CreateAsync(int orderId, IEnumerable<OrderProduct> orderProducts)
    {
        await _dbConnection.ExecuteAsync(@"
            INSERT INTO [dbo].[OrderProduct](OrderId, ProductId, Quantity)
            VALUES(@OrderId, @ProductId, @Quantity);",
           orderProducts.Select(x => new
           {
               OrderId = orderId,
               x.ProductId,
               x.Quantity
           }));
    }
}
