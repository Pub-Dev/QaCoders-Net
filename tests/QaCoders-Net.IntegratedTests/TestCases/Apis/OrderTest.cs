using NUnit.Framework;
using QaCodersNet.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QaCoders_Net.IntegratedTests.TestCases.Apis;

[TestFixture]
internal class OrderTest : BaseTest
{
    private readonly IOrderClient _orderClient;
    private readonly IProductClient _productClient;
    private readonly IClientClient _clientClient;

    public OrderTest()
    {
        _orderClient = TestHost<Startup>.GetService<IOrderClient>();
        _productClient = TestHost<Startup>.GetService<IProductClient>();
        _clientClient = TestHost<Startup>.GetService<IClientClient>();
    }

    [Test]
    public async Task CreateOrderAsync_WithClientIdThatNotExists_ReturnsBadRequest()
    {
        // prepare
        var orderCreateRequest = new OrderCreateRequest()
        {
            ClientId = int.MaxValue,
            Products = new List<OrderProductRequest>()
        };

        // act
        var response = await _orderClient.CreateOrderAsync(orderCreateRequest);

        // assert
        var errorResponse = await response.Error.GetContentAsAsync<ErrorResponse>();
        var error = errorResponse.Messages.FirstOrDefault();

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        Assert.IsTrue(error.ErrorCode == "CLIENT_NOT_FOUND");
        Assert.IsTrue(error.Message == $"Client {orderCreateRequest.ClientId} not found");
    }

    [Test]
    public async Task CreateOrderAsync_WithEmptyListOfProducts_ReturnsBadRequest()
    {
        // prepare
        var client = new ClientCreateRequest() { Name = "Test" };
        var clientCreateResponse = await _clientClient.CreateClientAsync(client);
        var orderCreateRequest = new OrderCreateRequest()
        {
            ClientId = clientCreateResponse.Content.ClientId,
            Products = new List<OrderProductRequest>()
        };

        // act
        var response = await _orderClient.CreateOrderAsync(orderCreateRequest);

        // assert
        var errorResponse = await response.Error.GetContentAsAsync<ErrorResponse>();
        var error = errorResponse.Messages.FirstOrDefault();

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        Assert.IsTrue(error.ErrorCode == "ORDER_EMPTY");
        Assert.IsTrue(error.Message == $"Order should have at least one product");
    }

    [Test]
    public async Task CreateOrderAsync_WithListOfProductsWithRepeatedProducts_ReturnsBadRequest()
    {
        // prepare
        var client = new ClientCreateRequest() { Name = "Test" };
        var clientCreateResponse = await _clientClient.CreateClientAsync(client);
        var orderCreateRequest = new OrderCreateRequest()
        {
            ClientId = clientCreateResponse.Content.ClientId,
            Products = new List<OrderProductRequest>()
            {
                new OrderProductRequest() { ProductId = 1 },
                new OrderProductRequest() { ProductId = 1 },
            }
        };

        // act
        var response = await _orderClient.CreateOrderAsync(orderCreateRequest);

        // assert
        var errorResponse = await response.Error.GetContentAsAsync<ErrorResponse>();
        var error = errorResponse.Messages.FirstOrDefault();

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        Assert.IsTrue(error.ErrorCode == "ORDER_PRODUCT_REPEATED");
        Assert.IsTrue(error.Message == $"Order should have a list of distict products");
    }

    [Test]
    public async Task CreateOrderAsync_WithProductThatDoesNotExists_ReturnsBadRequest()
    {
        // prepare
        var client = new ClientCreateRequest() { Name = "Test" };
        var clientCreateResponse = await _clientClient.CreateClientAsync(client);
        var orderCreateRequest = new OrderCreateRequest()
        {
            ClientId = clientCreateResponse.Content.ClientId,
            Products = new List<OrderProductRequest>()
            {
                new OrderProductRequest() { ProductId = int.MaxValue, Quantity = 1 },
            }
        };

        // act
        var response = await _orderClient.CreateOrderAsync(orderCreateRequest);

        // assert
        var errorResponse = await response.Error.GetContentAsAsync<ErrorResponse>();
        var error = errorResponse.Messages.FirstOrDefault();

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        Assert.IsTrue(error.ErrorCode == "PRODUCT_NOT_FOUND");
        Assert.IsTrue(error.Message == $"Product {int.MaxValue} not found");
    }

    [Test]
    public async Task CreateOrderAsync_WithProductWithInvalidQuantity_ReturnsBadRequest()
    {
        // prepare
        var client = new ClientCreateRequest() { Name = "Test" };
        var clientCreateResponse = await _clientClient.CreateClientAsync(client);
        var product = new ProductCreateRequest() { Name = "Product Test", Value = 15 };
        var productCreateResponse = await _productClient.CreateProductAsync(product);

        var orderCreateRequest = new OrderCreateRequest()
        {
            ClientId = clientCreateResponse.Content.ClientId,
            Products = new List<OrderProductRequest>()
            {
                new OrderProductRequest()
                {
                    ProductId = productCreateResponse.Content.ProductId,
                    Quantity = 0
                },
            }
        };

        // act
        var response = await _orderClient.CreateOrderAsync(orderCreateRequest);

        // assert
        var errorResponse = await response.Error.GetContentAsAsync<ErrorResponse>();
        var error = errorResponse.Messages.FirstOrDefault();

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        Assert.IsTrue(error.ErrorCode == "ORDER_PRODUCT_QUANTITY_ZERO");
        Assert.IsTrue(error.Message == $"Product {productCreateResponse.Content.ProductId} should have the Quantity greater than 0");
    }

    [Test]
    public async Task CreateOrderAsync_WithProductThatDoesNotExistsAndInvalidQuantity_ReturnsBadRequest()
    {
        // prepare
        var client = new ClientCreateRequest() { Name = "Test" };
        var clientCreateResponse = await _clientClient.CreateClientAsync(client);
        var orderCreateRequest = new OrderCreateRequest()
        {
            ClientId = clientCreateResponse.Content.ClientId,
            Products = new List<OrderProductRequest>()
            {
                new OrderProductRequest() { ProductId = int.MaxValue, Quantity = 0 },
            }
        };

        // act
        var response = await _orderClient.CreateOrderAsync(orderCreateRequest);

        // assert
        var errorResponse = await response.Error.GetContentAsAsync<ErrorResponse>();

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        Assert.IsTrue(errorResponse.Messages.Count == 2);
        Assert.IsTrue(errorResponse.Messages[0].ErrorCode == "PRODUCT_NOT_FOUND");
        Assert.IsTrue(errorResponse.Messages[0].Message == $"Product {int.MaxValue} not found");
        Assert.IsTrue(errorResponse.Messages[1].ErrorCode == "ORDER_PRODUCT_QUANTITY_ZERO");
        Assert.IsTrue(errorResponse.Messages[1].Message == $"Product {int.MaxValue} should have the Quantity greater than 0");
    }

    [Test]
    public async Task CreateOrderAsync_WithValidProductsAndClient_ReturnsCreated()
    {
        // prepare
        var client = new ClientCreateRequest() { Name = "Test" };
        var clientCreateResponse = await _clientClient.CreateClientAsync(client);
        var productOne = new ProductCreateRequest() { Name = "Product Test 1", Value = 20 };
        var productOneCreateResponse = await _productClient.CreateProductAsync(productOne);
        var productOneQuantity = 2;
        var productTwo = new ProductCreateRequest() { Name = "Product Test 2", Value = 35 };
        var productTwoCreateResponse = await _productClient.CreateProductAsync(productTwo);
        var productTwoQuantity = 5;
        var orderCreateRequest = new OrderCreateRequest()
        {
            ClientId = clientCreateResponse.Content.ClientId,
            Products = new List<OrderProductRequest>()
            {
                new OrderProductRequest()
                {
                    ProductId = productOneCreateResponse.Content.ProductId,
                    Quantity = productOneQuantity
                },
                new OrderProductRequest()
                {
                    ProductId = productTwoCreateResponse.Content.ProductId,
                    Quantity = productTwoQuantity
                },
            }
        };
        // act
        var response = await _orderClient.CreateOrderAsync(orderCreateRequest);

        // assert
        var orderCreateResponse = response.Content;
        var totalProductOne = productOne.Value * productOneQuantity;
        var totalProductTwo = productTwo.Value * productTwoQuantity;
        var total = totalProductOne + totalProductTwo;
        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.Created);
        Assert.IsTrue(orderCreateResponse.Total == total);
        Assert.IsTrue(orderCreateResponse.Client.ClientId == clientCreateResponse.Content.ClientId);
        Assert.IsTrue(orderCreateResponse.Items.Count == 2);
        Assert.IsTrue(orderCreateResponse.Items[0].Product.ProductId == productOneCreateResponse.Content.ProductId);
        Assert.IsTrue(orderCreateResponse.Items[0].Quantity == productOneQuantity);
        Assert.IsTrue(orderCreateResponse.Items[0].Value == totalProductOne);
        Assert.IsTrue(orderCreateResponse.Items[1].Product.ProductId == productTwoCreateResponse.Content.ProductId);
        Assert.IsTrue(orderCreateResponse.Items[1].Quantity == productTwoQuantity);
        Assert.IsTrue(orderCreateResponse.Items[1].Value == totalProductTwo);
    }

    [Test]
    public async Task GetOrderByIdAsync_WithOrderCreated_ReturnsOk()
    {
        // prepare
        var client = new ClientCreateRequest() { Name = "Test" };
        var clientCreateResponse = await _clientClient.CreateClientAsync(client);
        var product = new ProductCreateRequest() { Name = "Product Test", Value = 20 };
        var productCreateResponse = await _productClient.CreateProductAsync(product);
        var orderCreateRequest = new OrderCreateRequest()
        {
            ClientId = clientCreateResponse.Content.ClientId,
            Products = new List<OrderProductRequest>()
            {
                new OrderProductRequest()
                {
                    ProductId = productCreateResponse.Content.ProductId,
                    Quantity = 1
                }
            }
        };
        var orderCreateResponse = await _orderClient.CreateOrderAsync(orderCreateRequest);

        // act
        var response = await _orderClient.GetOrderByIdAsync(orderCreateResponse.Content.OrderId);

        // assert
        var orderGetByIdResponse = response.Content;
        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
        Assert.IsTrue(orderGetByIdResponse.Client.ClientId == clientCreateResponse.Content.ClientId);
        Assert.IsTrue(orderGetByIdResponse.Items.Count == 1);
    }
}
