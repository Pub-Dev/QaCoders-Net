using QaCoders_Net.Entities;

namespace QaCoders_Net.Requests;

public class ClientPatchRequest
{
    public string Name { get; set; }

    public static explicit operator Client(ClientPatchRequest clientPatchRequest)
    {
        return new()
        {
            Name = clientPatchRequest.Name
        };
    }
}

