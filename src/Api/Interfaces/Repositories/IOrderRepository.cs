using QaCoders_Net.Entities;

namespace QaCoders_Net.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();

    Task<Order> GetByIdAsync(int orderId);

    Task<Order> CreateAsync(Order order);
}
