using QaCoders_Net.Entities;
using QaCoders_Net.Interfaces.Repositories;
using QaCoders_Net.Interfaces.Services;

namespace QaCoders_Net.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _clientRepository.GetAllAsync();
    }

    public async Task<Client> GetByIdAsync(int clientId)
    {
        return await _clientRepository.GetByIdAsync(clientId);
    }

    public async Task<Client> CreateAsync(Client client)
    {
        return await _clientRepository.CreateAsync(client);
    }

    public async Task<Client> UpdateAsync(Client client)
    {
        return await _clientRepository.UpdateAsync(client);
    }
}
