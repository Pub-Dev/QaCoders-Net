﻿using QaCoders_Net.Entities;

namespace QaCoders_Net.Response;

public class ProductGetAllResponse
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }
    public DateTime CreatedDate { get; set; }

    public static explicit operator ProductGetAllResponse(Product product)
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