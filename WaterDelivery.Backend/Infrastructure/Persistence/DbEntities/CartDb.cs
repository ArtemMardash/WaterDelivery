namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

public class CartDb
{
    /// <summary>
    /// Customer Id
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Dictionary of products Ids and quantities
    /// </summary>
    public Dictionary<Guid, int> Items { get; set; } = new Dictionary<Guid, int>();
}