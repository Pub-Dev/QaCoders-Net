using QaCoders_Net.Entities;

namespace QaCoders_Net.Request;

public class ProductCreateRequest
{
    public string Name { get; set; }
    public decimal Value { get; set; }

    public static explicit operator Product(ProductCreateRequest productCreateRequest)
    {
        return new()
        {
            Name = productCreateRequest.Name,
            Value = productCreateRequest.Value
        };
    }
}