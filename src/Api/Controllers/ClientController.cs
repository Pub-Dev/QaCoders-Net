using Microsoft.AspNetCore.Mvc;
using QaCoders_Net.Entities;
using QaCoders_Net.Interfaces.Presenters;
using QaCoders_Net.Interfaces.Services;
using QaCoders_Net.Requests;
using QaCoders_Net.Responses;
using System.Net;

namespace QaCoders_Net.Controllers;

[ApiController]
[Route("clients")]
public class ClientController : ControllerBase
{
    private readonly IPresenter _presenter;
    private readonly IClientService _clientService;

    public ClientController(
        IPresenter presenter,
        IClientService clientService)
    {
        _presenter = presenter;
        _clientService = clientService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ClientGetAllResponse[]), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetClientAllAsync()
    {
        var data = await _clientService.GetAllAsync();

        return _presenter.GetResult(data, data => data.Select(client => (ClientGetAllResponse)client).ToArray());
    }


    [HttpGet("{clientId}")]
    [ActionName(nameof(GetClientByIdAsync))]
    [ProducesResponseType(typeof(ClientGetByIdResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetClientByIdAsync(int clientId)
    {
        var data = await _clientService.GetByIdAsync(clientId);

        return _presenter.GetResult(data, data => (ClientGetByIdResponse)data);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ClientCreateResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateClientAsync(ClientCreateRequest request)
    {
        var client = (Client)request;

        var data = await _clientService.CreateAsync(client);

        return _presenter.CreateResult(
            data,
            data => (ClientCreateResponse)data, (data) =>
            (nameof(GetClientByIdAsync), "Client", new { data.ClientId }));

    }

    [HttpPatch("{clientId}")]
    [ProducesResponseType(typeof(ClientPatchResponse), (int)HttpStatusCode.Accepted)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> PatchClientAsync(int clientId, ClientPatchRequest request)
    {
        var client = (Client)request;

        client.ClientId = clientId;

        var data = await _clientService.UpdateAsync(client);

        return _presenter.AcceptedResult(
            data,
            data => (ClientPatchResponse)data, (data) =>
            (nameof(GetClientByIdAsync), "Client", new { data.ClientId }));
    }
}
