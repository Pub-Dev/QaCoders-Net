using NUnit.Framework;
using QaCoders_Net.Entities;
using QaCoders_Net.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace QaCoders_Net.IntegratedTests.TestCases.Repositories;

[TestFixture]
public class OrderRepositoryTest : BaseTest
{
    private readonly OrderRepository _orderRepository;
    private readonly ClientRepository _clientRepository;

    public OrderRepositoryTest()
    {
        _orderRepository = new OrderRepository(TestHost<Startup>.GetService<IDbConnection>());
        _clientRepository = new ClientRepository(TestHost<Startup>.GetService<IDbConnection>());
    }

    [Test]
    public void GetAllAsync_Check()
    {
        // assert
        Assert.DoesNotThrowAsync(() => _orderRepository.GetAllAsync());
    }

    [Test]
    public void GetByIdAsync_Check()
    {
        // assert
        Assert.DoesNotThrowAsync(() => _orderRepository.GetByIdAsync(1));
    }

    [Test]
    public async Task CreateAsync_Check()
    {
        // prepare
        var client = await _clientRepository.CreateAsync(new Client { Name = "Test" });
        var order = new Order() { ClientId = client.ClientId };

        // assert
        Assert.DoesNotThrowAsync(() => _orderRepository.CreateAsync(order));
    }
}


[TestFixture]
public class OrderProductRepositoryTest : BaseTest
{
    private readonly OrderProductRepository _orderProductRepository;
    private readonly ProductRepository _productRepository;
    private readonly OrderRepository _orderRepository;
    private readonly ClientRepository _clientRepository;

    public OrderProductRepositoryTest()
    {
        _orderProductRepository = new OrderProductRepository(TestHost<Startup>.GetService<IDbConnection>());
        _productRepository = new ProductRepository(TestHost<Startup>.GetService<IDbConnection>());
        _orderRepository = new OrderRepository(TestHost<Startup>.GetService<IDbConnection>());
        _clientRepository = new ClientRepository(TestHost<Startup>.GetService<IDbConnection>());
    }

    [Test]
    public async Task CreateAsync_Check()
    {
        // prepate
        var client = await _clientRepository.CreateAsync(new Client() { Name = "this is a test" });
        var product = await _productRepository.CreateAsync(new Product() { Name = "this is a test", Value = 15 });
        var order = await _orderRepository.CreateAsync(new Order() { ClientId = client.ClientId });
        var orderProducts = new List<OrderProduct>()
        {
            new OrderProduct()
            {
                ProductId = product.ProductId,
                Quantity = 1
            }
        };

        // assert
        Assert.DoesNotThrowAsync(() => _orderProductRepository.CreateAsync(order.OrderId, orderProducts));
    }
}