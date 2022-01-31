using QaCoders_Net.Entities;

namespace QaCoders_Net.Responses;

public class ProductPatchResponse
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }
    public DateTime CreatedDate { get; set; }

    public static explicit operator ProductPatchResponse(Product product)
    {
        return new()
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Value = product.Value,
            CreatedDate = product.CreateDate
        };
    }
}
