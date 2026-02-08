using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

public static class DbToDomain
{
    public static Address ToDomain(this AddressDb addressDb)
    {
        return new Address(addressDb.Id, addressDb.Street, addressDb.HouseNumber, addressDb.AptNumber, addressDb.City,
            addressDb.State, addressDb.isDeleted);
    }

    public static Bill ToDomain(this BillDb billDb)
    {
        return new Bill(billDb.Id, billDb.Order.ToDomain(), billDb.CreationDate, billDb.PaymentDate,
            (BillStatus)billDb.Status);
    }

    public static Order ToDomain(this OrderDb orderDb)
    {
        return new Order(orderDb.Id, orderDb.CustomerId, orderDb.Items.Select(o => o.ToDomain()).ToList());
    }

    public static OrderItem ToDomain(this OrderItemDb orderItemDb)
    {
        return new OrderItem(orderItemDb.Product.ToDomain(), orderItemDb.Quantity, orderItemDb.ProductUnit.ToDomain());
    }

    public static Product ToDomain(this ProductDb productDb)
    {
        return new Product(productDb.Id, productDb.Name, productDb.Description,
            productDb.ProductOptions.Select(p => p.ToDomain()).ToList(), productDb.DefaultUnit.ToDomain(),
            productDb.DefaultUnitPrice);
    }

    public static ProductUnit ToDomain(this ProductUnitDb productUnitDb)
    {
        return new ProductUnit(productUnitDb.Id, (MeasurementUnits)productUnitDb.Name,
            productUnitDb.QuantityPerUnit);
    }

    public static User ToDomain(this UserDb userDb)
    {
        return new User(userDb.Id, userDb.Name, (UserType)userDb.UserType, userDb.Email, userDb.PhoneNumber);
    }

    public static CustomerAddresses ToDomain(this CustomerAddressesDb customerAddressesDb)
    {
        return new CustomerAddresses(customerAddressesDb.Id, customerAddressesDb.CustomerId,
            customerAddressesDb.Addresses.Select(a => a.ToDomain()).ToList());
    }

    public static Delivery ToDomain(this DeliveryDb deliveryDb)
    {
        return new Delivery(deliveryDb.Id, deliveryDb.DeliveryManId, deliveryDb.Order.ToDomain(),
            deliveryDb.Address.ToDomain(), (DeliveryStatus)deliveryDb.Status);
    }
}