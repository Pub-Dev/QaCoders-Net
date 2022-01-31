using QaCoders_Net.Entities;

namespace QaCoders_Net.Requests;

public class ClientCreateRequest
{
    public string Name { get; set; }

    public static explicit operator Client(ClientCreateRequest clientCreateRequest)
    {
        return new()
        {
            Name = clientCreateRequest.Name
        };
    }
}
