namespace WaterDelivery.Backend.Core.Entities;

public class OrderItem
{
    public Product Product { get; set; }

    public int Quantity { get; set; }

    public ProductUnit ProductUnit { get; set; }

    public OrderItem(Product product, int quantity, ProductUnit productUnit)
    {
        Product = product ?? throw new ArgumentNullException(nameof(product), "Invalid product");
        Quantity = quantity <= 0 ? throw new ArgumentException("Quantity can not be 0", nameof(Quantity)) : quantity;
        ProductUnit = productUnit ?? throw new ArgumentNullException(nameof(productUnit), "Invalid product unit");
    }
}