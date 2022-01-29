using QaCoders_Net.Entities;

namespace QaCoders_Net.Response;

public class ClientPatchResponse
{
    public int ClientId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }

    public static explicit operator ClientPatchResponse(Client client)
    {
        return new()
        {
            ClientId = client.ClientId,
            Name = client.Name,
            CreatedDate = client.CreateDate
        };
    }
}
