using QaCoders_Net.Entities;

namespace QaCoders_Net.Interfaces.Repositories;

public interface IOrderProductRepository
{
    Task CreateAsync(int orderId, IEnumerable<OrderProduct> orderProducts);
}