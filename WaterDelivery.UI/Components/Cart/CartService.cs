namespace WaterDelivery.UI.Components.Cart;

public class CartService
{
    private readonly List<CartItem> _items = new();
 
    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
 
    public decimal Total => _items.Sum(x => x.Price * x.Quantity);
 
    public int Count => _items.Sum(x => x.Quantity);
 
    public event Action? OnChange;
 
    public void AddToCart(CartItem product)
    {
        var existing = _items.FirstOrDefault(x => x.ProductId == product.ProductId);
        if (existing is null)
            _items.Add(product);
        else
            existing.Quantity++;
 
        NotifyStateChanged();
    }
 
    public void IncreaseQuantity(Guid productId)
    {
        var item = _items.FirstOrDefault(x => x.ProductId == productId);
        if (item is not null) { item.Quantity++; NotifyStateChanged(); }
    }
 
    public void DecreaseQuantity(Guid productId)
    {
        var item = _items.FirstOrDefault(x => x.ProductId == productId);
        if (item is null) return;
        item.Quantity--;
        if (item.Quantity <= 0) _items.Remove(item);
        NotifyStateChanged();
    }
 
    public void RemoveFromCart(Guid productId)
    {
        var item = _items.FirstOrDefault(x => x.ProductId == productId);
        if (item is not null) { _items.Remove(item); NotifyStateChanged(); }
    }
 
    public void Clear()
    {
        _items.Clear();
        NotifyStateChanged();
    }
 
    private void NotifyStateChanged() => OnChange?.Invoke();
}
 
public class CartItem
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}