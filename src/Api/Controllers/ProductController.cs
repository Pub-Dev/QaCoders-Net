using Microsoft.AspNetCore.Mvc;
using QaCoders_Net.Entities;
using QaCoders_Net.Interfaces.Services;
using QaCoders_Net.Request;
using QaCoders_Net.Response;
using System.Net;

namespace QaCoders_Net.Controllers;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProductGetAllResponse[]), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetProductAllAsync()
    {
        var data = await _productService.GetAllAsync();

        var response = data.Select(x => (ProductGetAllResponse)x).ToArray();

        return Ok(response);
    }

    [HttpGet("{productId}")]
    [ActionName(nameof(GetProductByIdAsync))]
    [ProducesResponseType(typeof(ProductGetByIdResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]    
    public async Task<IActionResult> GetProductByIdAsync(int productId)
    {
        var data = await _productService.GetByIdAsync(productId);

        var response = (ProductGetByIdResponse)data;

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductCreateResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateProductAsync(ProductCreateRequest request)
    {
        var client = (Product)request;

        var data = await _productService.CreateAsync(client);

        var response = (ProductCreateResponse)data;

        return CreatedAtAction(nameof(GetProductByIdAsync), new { response.ProductId }, response);
    }

    [HttpPatch("{productId}")]
    [ProducesResponseType(typeof(ProductPatchResponse), (int)HttpStatusCode.Accepted)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> PatchProductAsync(int productId, ProductPatchRequest request)
    {
        var product = (Product)request;

        product.ProductId = productId;

        var data = await _productService.UpdateAsync(product);

        var response = (ProductPatchResponse)data;

        return Accepted(response);
    }
}
