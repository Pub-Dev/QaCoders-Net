using QaCoders_Net.Entities;

namespace QaCoders_Net.Responses;

public class ClientResponse
{
    public int ClientId { get; set; }
    public string Name { get; set; }

    public static explicit operator ClientResponse(Client client)
    {
        return new()
        {
            ClientId = client.ClientId,
            Name = client.Name,
        };
    }
}
