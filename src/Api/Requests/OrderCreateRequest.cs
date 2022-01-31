using QaCoders_Net.Entities;

namespace QaCoders_Net.Requests;

public class OrderCreateRequest
{
    public int ClientId { get; set; }
    public OrderProductRequest[] Products { get; set; }

    public static explicit operator Order(OrderCreateRequest orderCreateRequest)
    {
        return new()
        {
            ClientId = orderCreateRequest.ClientId,
            OrderProducts = orderCreateRequest.Products.Select(x => (OrderProduct)x).ToList()
        };
    }
}
