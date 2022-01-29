namespace QaCoders_Net.Entities;

public class Client : BaseEntity
{
    public int ClientId { get; set; }
    public string Name { get; set; }
    public ICollection<Order> Orders = new HashSet<Order>();
}
