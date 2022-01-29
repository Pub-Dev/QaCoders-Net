using Microsoft.AspNetCore.Mvc;
using QaCoders_Net.Entities;
using QaCoders_Net.Interfaces.Services;
using QaCoders_Net.Request;
using QaCoders_Net.Response;
using System.Net;

namespace QaCoders_Net.Controllers;

[ApiController]
[Route("clients")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ClientGetByIdResponse[]), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetClientAllAsync()
    {
        var data = await _clientService.GetAllAsync();

        var response = data.Select(x => (ClientGetAllResponse)x).ToArray();

        return Ok(response);
    }


    [HttpGet("{clientId}")]
    [ActionName(nameof(GetClientByIdAsync))]
    [ProducesResponseType(typeof(ClientGetByIdResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetClientByIdAsync(int clientId)
    {
        var data = await _clientService.GetByIdAsync(clientId);

        var response = (ClientGetByIdResponse)data;

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ClientCreateResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateClientAsync(ClientCreateRequest request)
    {
        var client = (Client)request;

        var data = await _clientService.CreateAsync(client);

        var response = (ClientCreateResponse)data;

        return CreatedAtAction(nameof(GetClientByIdAsync), new { response.ClientId }, response);
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

        var response = (ClientPatchResponse)data;

        return Accepted(response);
    }
}
