using QaCoders_Net.Entities;

namespace QaCoders_Net.Requests;

public class OrderProductRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public static explicit operator OrderProduct(OrderProductRequest orderProductRequest)
    {
        return new()
        {
            ProductId = orderProductRequest.ProductId,
            Quantity = orderProductRequest.Quantity
        };
    }
}