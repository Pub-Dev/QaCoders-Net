using QaCoders_Net.Entities;

namespace QaCoders_Net.Request;

public class ProductPatchRequest
{
    public string Name { get; set; }
    public decimal Value { get; set; }

    public static explicit operator Product(ProductPatchRequest productPatchRequest)
    {
        return new()
        {
            Name = productPatchRequest.Name,
            Value = productPatchRequest.Value
        };
    }
}
