using QaCoders_Net.Entities;
using QaCoders_Net.Enums;
using QaCoders_Net.Interfaces.Repositories;
using QaCoders_Net.Interfaces.Services;

namespace QaCoders_Net.Services;

public class ClientService : IClientService
{
    private readonly NotificationContext _notificationContext;
    private readonly IClientRepository _clientRepository;

    public ClientService(
        NotificationContext notificationContext,
        IClientRepository clientRepository)
    {
        _notificationContext = notificationContext;
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _clientRepository.GetAllAsync();
    }

    public async Task<Client?> GetByIdAsync(int clientId)
    {
        var client = await _clientRepository.GetByIdAsync(clientId);

        if (client is not null)
        {
            return client;
        }

        _notificationContext.AddNotification("CLIENT_NOT_FOUND", $"Client {clientId} not found", ErrorType.NotFound);

        return null;
    }

    public async Task<Client?> CreateAsync(Client client)
    {
        return await _clientRepository.CreateAsync(client);
    }

    public async Task<Client?> UpdateAsync(Client client)
    {
        var clientData = await _clientRepository.GetByIdAsync(client.ClientId);

        if (clientData is not null)
        {
            return await _clientRepository.UpdateAsync(client);
        }

        _notificationContext.AddNotification("CLIENT_NOT_FOUND", $"Client {client.ClientId} not found", ErrorType.Validation);

        return null;
    }
}
