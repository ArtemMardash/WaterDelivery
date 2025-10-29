namespace WaterDelivery.Backend.Core.Entities;

public class Product
{
    private const int NAME_MIN_LENGTH = 5;
    private const int DESCRIPTION_MIN_LENGTH = 20;
    private const int NAME_MAX_LENGTH = 40;
    private const int DESCRIPTION_MAX_LENGTH = 150;

    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    /// <summary>
    /// Options for sale
    /// </summary>
    public List<ProductUnit> ProductOptions { get; set; }

    public ProductUnit DefaultUnit { get; set; }

    public decimal DefaultUnitPrice { get; set; }

    public Product(Guid id, string name, string description, List<ProductUnit> productOptions, ProductUnit defaultUnit,
        decimal defaultUnitPrice)
    {
        Id = id;
        SetName(name);
        SetDescription(description);
        ProductOptions = productOptions ?? throw new ArgumentNullException(nameof(productOptions), "Product options can not be empty");
        DefaultUnit = defaultUnit ?? throw new ArgumentNullException(nameof(defaultUnit), "Invalid default unit");
        DefaultUnitPrice = defaultUnitPrice <= 0
            ? throw new ArgumentException("Default price cannot be less or equal to 0", nameof(DefaultUnitPrice))
            : defaultUnitPrice;
    }

    public Product(string name, string description, List<ProductUnit> productOptions, ProductUnit defaultUnit,
        decimal defaultUnitPrice)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetDescription(description);
        ProductOptions = productOptions ?? throw new ArgumentNullException(nameof(productOptions), "Product options can not be empty");
        DefaultUnit = defaultUnit ?? throw new ArgumentNullException(nameof(defaultUnit), "Invalid default unit");
        DefaultUnitPrice = defaultUnitPrice <= 0
            ? throw new ArgumentException("Default price cannot be less or equal to 0", nameof(DefaultUnitPrice))
            : defaultUnitPrice;
    }

    private void SetName(string input)
    {
        if (string.IsNullOrWhiteSpace(input) ||input.Length < NAME_MIN_LENGTH || input.Length > NAME_MAX_LENGTH)
        {
            throw new ArgumentException(
                $"Name should be longer then {NAME_MIN_LENGTH} and shorter then {NAME_MAX_LENGTH}", nameof(Name));
        }

        Name = input;
    }
    
    private void SetDescription(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length < DESCRIPTION_MIN_LENGTH || input.Length > DESCRIPTION_MAX_LENGTH)
        {
            throw new ArgumentException(
                $"Description should be longer then {DESCRIPTION_MIN_LENGTH} and shorter then {DESCRIPTION_MAX_LENGTH}", nameof(Description));
        }

        Description = input;
    }
}