using QaCoders_Net.Entities;

namespace QaCoders_Net.Interfaces.Services;

public interface IOrderService 
{
    Task<IEnumerable<Order>> GetAllAsync();

    Task<Order?> GetByIdAsync(int orderId);

    Task<Order?> CreateAsync(Order order);
}