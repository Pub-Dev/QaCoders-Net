using NUnit.Framework;
using QaCodersNet.Client;
using System.Linq;
using System.Threading.Tasks;

namespace QaCoders_Net.IntegratedTests.TestCases.Apis;

[TestFixture]
public class ProductTest : BaseTest
{
    private readonly IProductClient _productClient;

    public ProductTest()
    {
        _productClient = TestHost<Startup>.GetService<IProductClient>();
    }

    [Test]
    public async Task GetProductByIdAsync_WithProductIdThatNotExists_ReturnsNotFound()
    {
        // prepare
        var productId = int.MaxValue;

        // act
        var response = await _productClient.GetProductByIdAsync(productId);

        // assert
        var errorResponse = await response.Error.GetContentAsAsync<ErrorResponse>();
        var error = errorResponse.Messages.FirstOrDefault();

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        Assert.IsTrue(error.ErrorCode == "PRODUCT_NOT_FOUND");
        Assert.IsTrue(error.Message == $"Product {productId} not found");
    }

    [Test]
    public async Task CreateClientAsync_CreateWithCorrectData_ReturnsCreated()
    {
        // prepare
        var product = new ProductCreateRequest() { Name = "Test", Value = 10 };

        // act
        var response = await _productClient.CreateProductAsync(product);

        // assert
        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.Created);
        Assert.NotNull(response.Content);
        Assert.IsTrue(response.Content.Name == product.Name);
        Assert.IsTrue(response.Content.Value == product.Value);
    }

    [Test]
    public async Task GetProductAllAsync_ReturnsProducts()
    {
        // prepare
        var product = new ProductCreateRequest() { Name = "Test", Value = 10 };
        var productCreateResponse = await _productClient.CreateProductAsync(product);
        var productCreated = productCreateResponse.Content;

        // act
        var response = await _productClient.GetProductAllAsync();

        // assert
        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
        Assert.NotNull(response.Content);
        Assert.IsTrue(response.Content.Any(x => x.ProductId == productCreated.ProductId));
    }

    [Test]
    public async Task PatchProductAsync_WithProductThatDoesNotExists_ReturnsBadRequest()
    {
        // prepare        
        var productId = int.MaxValue;
        var productPatchRequest = new ProductPatchRequest() { Name = "New Name", Value = 15 };

        // act
        var response = await _productClient.PatchProductAsync(productId, productPatchRequest);

        // assert
        var errorResponse = await response.Error.GetContentAsAsync<ErrorResponse>();
        var error = errorResponse.Messages.FirstOrDefault();

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        Assert.IsTrue(error.ErrorCode == "PRODUCT_NOT_FOUND");
        Assert.IsTrue(error.Message == $"Product {productId} not found");
    }

    [Test]
    public async Task PatchClientAsync_WithValidInformation_ReturnsAccepted()
    {
        // prepare
        var product = new ProductCreateRequest() { Name = "Test", Value = 10 };
        var productCreateResponse = await _productClient.CreateProductAsync(product);
        var productCreated = productCreateResponse.Content;
        var productPatchRequest = new ProductPatchRequest() { Name = "New Name", Value = 60 };

        // act
        var response = await _productClient.PatchProductAsync(productCreated.ProductId, productPatchRequest);

        // assert
        var patchProductResponse = response.Content;

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.Accepted);
        Assert.IsTrue(patchProductResponse.ProductId == productCreated.ProductId);
        Assert.IsTrue(patchProductResponse.Name == productPatchRequest.Name);
        Assert.IsTrue(patchProductResponse.Value == productPatchRequest.Value);
    }
}