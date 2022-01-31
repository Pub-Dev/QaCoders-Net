using NUnit.Framework;
using QaCodersNet.Client;
using System.Linq;
using System.Threading.Tasks;

namespace QaCoders_Net.IntegratedTests.TestCases.Apis;

[TestFixture]
public class ClientTest : BaseTest
{
    private readonly IClientClient _clientClient;

    public ClientTest()
    {
        _clientClient = TestHost<Startup>.GetService<IClientClient>();
    }

    [Test]
    public async Task GetClientByIdAsync_WithClientIdThatNotExists_ReturnsNotFound()
    {
        // prepare
        var clientId = int.MaxValue;

        // act
        var response = await _clientClient.GetClientByIdAsync(clientId);

        // assert
        var errorResponse = await response.Error.GetContentAsAsync<ErrorResponse>();
        var error = errorResponse.Messages.FirstOrDefault();

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        Assert.IsTrue(error.ErrorCode == "CLIENT_NOT_FOUND");
        Assert.IsTrue(error.Message == $"Client {clientId} not found");
    }

    [Test]
    public async Task CreateClientAsync_CreateWithCorrectData_ReturnsCreated()
    {
        // prepare
        var client = new ClientCreateRequest() { Name = "Test" };

        // act
        var response = await _clientClient.CreateClientAsync(client);

        // assert
        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.Created);
        Assert.NotNull(response.Content);
        Assert.IsTrue(response.Content.Name == client.Name);
    }

    [Test]
    public async Task GetClientAllAsync_ReturnsClients()
    {
        // prepare
        var client = new ClientCreateRequest() { Name = "Test" };
        var clientCreateResponse = await _clientClient.CreateClientAsync(client);
        var clientCreated = clientCreateResponse.Content;

        // act
        var response = await _clientClient.GetClientAllAsync();

        // assert
        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
        Assert.NotNull(response.Content);
        Assert.IsTrue(response.Content.Any(x => x.ClientId == clientCreated.ClientId));
    }


    [Test]
    public async Task PatchClientAsync_WithClientThatDoesNotExists_ReturnsBadRequest()
    {
        // prepare        
        var clientId = int.MaxValue;
        var clientPatchRequest = new ClientPatchRequest() { Name = "New Name" };

        // act
        var response = await _clientClient.PatchClientAsync(clientId, clientPatchRequest);

        // assert
        var errorResponse = await response.Error.GetContentAsAsync<ErrorResponse>();
        var error = errorResponse.Messages.FirstOrDefault();

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        Assert.IsTrue(error.ErrorCode == "CLIENT_NOT_FOUND");
        Assert.IsTrue(error.Message == $"Client {clientId} not found");
    }

    [Test]
    public async Task PatchClientAsync_WithValidInformation_ReturnsAccepted()
    {
        // prepare
        var client = new ClientCreateRequest() { Name = "Test" };
        var clientCreateResponse = await _clientClient.CreateClientAsync(client);
        var clientCreated = clientCreateResponse.Content;
        var clientPatchRequest = new ClientPatchRequest() { Name = "New Name" };

        // act
        var response = await _clientClient.PatchClientAsync(clientCreated.ClientId, clientPatchRequest);

        // assert
        var patchClientResponse = response.Content;

        Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.Accepted);
        Assert.IsTrue(patchClientResponse.ClientId == clientCreated.ClientId);
        Assert.IsTrue(patchClientResponse.Name == clientPatchRequest.Name);
    }
}
