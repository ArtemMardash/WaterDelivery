using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.ValueObjects;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

public static class DomainToDb
{
    public static UserDb ToDb(this User user)
    {
        return new UserDb
        {
            Id = user.Id.ToString(),
            Name = user.Name,
            UserType = (int)user.UserType
        };
    }

    public static OrderDb ToDb(this Order order)
    {
        return new OrderDb
        {
            Id = order.Id.ToString(),
            CustomerId = order.CustomerId,
            Items = order.Items.Select(o => o.ToDb()).ToList()
        };
    }

    public static OrderItemDb ToDb(this OrderItem orderItem)
    {
        return new OrderItemDb
        {
            Product = orderItem.Product.ToDb(),
            Quantity = orderItem.Quantity,
            ProductUnit = orderItem.ProductUnit.ToDb()
        };
    }

    public static ProductDb ToDb(this Product product)
    {
        return new ProductDb
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Description = product.Description,
            ProductOptions = product.ProductOptions.Select(p => p.ToDb()).ToList(),
            DefaultUnit = product.DefaultUnit.ToDb(),
            DefaultUnitPrice = product.DefaultUnitPrice
        };
    }

    public static ProductUnitDb ToDb(this ProductUnit productUnit)
    {
        return new ProductUnitDb
        {
            Id = productUnit.Id.ToString(),
            Name = (int)productUnit.Name,
            QuantityPerUnit = productUnit.QuantityPerUnit
        };
    }

    public static AddressDb ToDb(this Address address)
    {
        return new AddressDb
        {
            Id = address.Id.ToString(),
            Street = address.Street,
            HouseNumber = address.HouseNumber,
            AptNumber = address.AptNumber,
            City = address.City,
            State = address.State
        };
    }

    public static BillDb ToDb(this Bill bill)
    {
        return new BillDb
        {
            Id = bill.Id.ToString(),
            Order = bill.Order.ToDb(),
            CreationDate = bill.CreationDate,
            PaymentDate = bill.PaymentDate,
            Status = (int)bill.Status
        };
    }

    public static DeliveryDb ToDb(this Delivery delivery)
    {
        return new DeliveryDb
        {
            Id = delivery.Id.ToString(),
            DeliveryManId = delivery.DeliveryManId,
            Order = delivery.Order.ToDb(),
            Address = delivery.Address.ToDb(),
            Status = (int)delivery.Status
        };
    }

    public static CustomerAddressesDb ToDb(this CustomerAddresses customerAddresses)
    {
        return new CustomerAddressesDb
        {
            Id = customerAddresses.Id.ToString(),
            CustomerId = customerAddresses.Id,
            Addresses = customerAddresses.Addresses.Select(a => a.ToDb()).ToList()
        };
    }
}