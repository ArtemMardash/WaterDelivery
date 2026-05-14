
namespace WaterDelivery.Backend.Core.Entities;
//ToDo think how to use ProductUnit entity 
public class Cart
{
    /// <summary>
    /// Customer Id
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Dictionary of products Ids and quantities
    /// </summary>
    public Dictionary<Guid, int> Items { get; set; } = new Dictionary<Guid, int>();

    public Cart(Guid customerId, Dictionary<Guid, int> items)
    {
        CustomerId = customerId;
        Items = items;
    }

    /// <summary>
    /// Method to get a total price for a cart
    /// </summary>
    /// <param name="itemsPrices"> map productId to productPrice</param>
    /// <returns></returns>
    public decimal TotalPrice(Dictionary<Guid, decimal> itemsPrices)
    {
        decimal totalPrice = 0;
        foreach (var kvp in Items)
        {
            if (!itemsPrices.Keys.Contains(kvp.Key))
            {
                continue;
            }

            totalPrice += kvp.Value * itemsPrices[kvp.Key];
        }

        return totalPrice;
    }
}