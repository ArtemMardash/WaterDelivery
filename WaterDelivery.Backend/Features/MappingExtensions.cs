using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Addresses.Dtos;
using WaterDelivery.Backend.Features.Orders.Dtos;
using WaterDelivery.Backend.Features.Products.Dtos;
using WaterDelivery.Backend.Features.ProductUnits.Dtos;

namespace WaterDelivery.Backend.Features;

public static class MappingExtensions
{
    public static OrderItem ToEntity(this OrderItemDto dto)
    {
        return new OrderItem(dto.Product.ToEntity(), dto.Quantity, dto.ProductUnit.ToEntity());
    }

    public static OrderItemDto ToDto(this OrderItem orderItem)
    {
        return new OrderItemDto
        {
            Product = orderItem.Product.ToDto(),
            Quantity = orderItem.Quantity,
            ProductUnit = orderItem.ProductUnit.ToDto()
        };
    }

    public static Product ToEntity(this ProductDto dto)
    {
        var productOptions = dto.ProductOptions.Select(pu => pu.ToEntity()).ToList();
        return new Product(dto.Name, dto.Description, productOptions, dto.DefaultUnit.ToEntity(), dto.DefaultUnitPrice);
    }

    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ProductOptions = product.ProductOptions.Select(pu => pu.ToDto()).ToList(),
            DefaultUnit = product.DefaultUnit.ToDto(),
            DefaultUnitPrice = product.DefaultUnitPrice
        };
    }

    public static ProductUnit ToEntity(this ProductUnitDto dto)
    {
        return new ProductUnit(dto.Id, dto.Name, dto.QuantityPerUnit);
    }

    public static ProductUnitDto ToDto(this ProductUnit productUnit)
    {
        return new ProductUnitDto
        {
            Id = productUnit.Id,
            Name = productUnit.Name,
            QuantityPerUnit = productUnit.QuantityPerUnit
        };
    }

    public static Order ToEntity(this OrderDto dto)
    {
        var items = dto.Items.Select(oi => oi.ToEntity()).ToList();
        return new Order(dto.Id, dto.CustomerId, items);
    }

    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Items = order.Items.Select(oi => oi.ToDto()).ToList()
        };
    }

    public static Address ToEntity(this AddressDto dto)
    {
        return new Address(dto.Id, dto.Street, dto.HouseNumber, dto.AptNumber, dto.City, dto.State, dto.isDeleted);
    }

    public static AddressDto ToDto(this Address address)
    {
        return new AddressDto
        {
            Id = address.Id,
            Street = address.Street,
            HouseNumber = address.HouseNumber,
            AptNumber = address.AptNumber,
            City = address.City,
            State = address.State,
            isDeleted = address.IsDeleted
        };
    }
}
