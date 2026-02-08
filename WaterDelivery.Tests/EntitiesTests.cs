using FluentAssertions;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;

namespace WaterDelivery.Tests;

public class EntitiesTests
{
    public static IEnumerable<object[]> DeliveryTestData =>
        new List<object[]>
        {
            new object[]
            {
                "Creat delivery with null order",
                Guid.NewGuid(),
                default(Order)!,
                new Address("street", "houseNum", "1B", "sdcdcdc", "sdvdvdsvd"),
                DeliveryStatus.Assembly
            },
            new object[]
            {
                "Creat delivery with null Address",
                Guid.NewGuid(),
                new Order(Guid.NewGuid(), new List<OrderItem>()),
                default(Address)!,
                DeliveryStatus.Assembly
            },
        };

    public static IEnumerable<object[]> BillTestData => new List<object[]>
    {
        new object[]
        {
            new Order(Guid.NewGuid(), new List<OrderItem>()),
            DateTime.Now,
            DateTime.Now.AddDays(-1),
            BillStatus.WaitForPayment
        },
        new object[]
        {
            new Order(Guid.NewGuid(), new List<OrderItem>()),
            DateTime.Now.AddDays(-1),
            DateTime.Now.AddDays(2),
            BillStatus.WaitForPayment
        },
        new object[]
        {
            new Order(Guid.NewGuid(), new List<OrderItem>()),
            DateTime.Now,
            DateTime.Now.AddDays(2),
            BillStatus.Unknown
        }
    };
    
    public static IEnumerable<object[]> OrderItemTestData =>
        new List<object[]>
        {
            new object[]
            {
                "Create OrderItems with incorrect quantity",
                new Product("asdfgh", "asscsddcdcscsccdscscsdscs", new List<ProductUnit>(),
                    new ProductUnit(MeasurementUnits.Gram, 10), 10),
                new ProductUnit(MeasurementUnits.Gram, 100),
                0
            },
            new object[]
            {
                "Create OrderItems with negative quantity",
                new Product("asdfgh", "asscsddcdcscsccdscscsdscs", new List<ProductUnit>(),
                    new ProductUnit(MeasurementUnits.Gram, 10), 10),
                new ProductUnit(MeasurementUnits.Gram, 100),
                -10
            },
            new object[]
            {
                "Create OrderItems with null product",
                default(Product)!,
                new ProductUnit(MeasurementUnits.Gram, 100),
                10
            },
            new object[]
            {
                "Create OrderItems with null product unit",
                new Product("asdfgh", "asscsddcdcscsccdscscsdscs", new List<ProductUnit>(),
                    new ProductUnit(MeasurementUnits.Gram, 10), 10),
                default(ProductUnit)!,
                10
            }
        };

    [Theory]
    [InlineData("asdf", UserType.Customer, "artem.m@gmail.com", "1231234123")]
    [InlineData("asdfdjvfdvdfvfdvfdvdfvdfvdfvdfvdfvfdvdfvdfvdfvdvfdsvsdfvfdvdcasdcdscsdcsccsdcscaca",
        UserType.Customer, "artem.m@gmail.com", "1231234123")]
    [InlineData("asdfdfv", UserType.Unknown, "artem.m@gmail.com", "1231234123")]
    [InlineData("asdfdfv", UserType.Customer, "artem.mgmail.com", "1231234123")]
    [InlineData("asdfdfv", UserType.Customer, "artem.mgmail.com", "12312341")]
    public void Create_User_Should_Fail(string name, UserType userType, string email, string phoneNumber)
    {
        var test = () => new User(name, userType, email,phoneNumber);

        test.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(MeasurementUnits.Liter, 0)]
    public void Create_Product_Unit_Should_Fail(MeasurementUnits measurementUnits, int quantity)
    {
        var test = () => new ProductUnit(measurementUnits, quantity);

        test.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("adfs", "afsaddvfdvfdvdfvdfvdfvdfvfv", 10)]
    [InlineData("adfsdfvdfvfdvdfvdfvfdvdvsvkskwemf;lscd;vmfdjlslvfdvssdc;svlkvmlkv", "afsaddvfdvfdvdfvdfvdfvdfvfv", 10)]
    [InlineData("adfsdf",
        "afsaddvfdvfdvdfvdfvdfvdfvfvsknvfdkvjnjvfvdfvdfvfdvdfvdfvdfvdfvdfvdfvdfvjsdvcl;msdlkcsdkvdlvndvldsa;s;cs;c,pds'v,asdvlllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll",
        10)]
    [InlineData("adfsfdv", "aqqqqqqq", 10)]
    [InlineData("adfsdsv", "afsaddvfdvfdvdfvdfvdfvdfvfv", 0)]
    [InlineData("adfsdsv", "afsaddvfdvfdvdfvdfvdfvdfvfv", -10)]
    [InlineData(null, "afsaddvfdvfdvdfvdfvdfvdfvfv", -10)]
    [InlineData("adfsfdv", null, 10)]
    public void Create_Product_Should_Fail(string name, string descrption, decimal defaultPrice)
    {
        var test = () => new Product(name, descrption, new List<ProductUnit>(),
            new ProductUnit(MeasurementUnits.Liter, 10), defaultPrice);

        test.Should().Throw<ArgumentException>();
    }

    [Theory]
    [MemberData(nameof(OrderItemTestData))]
    public void Create_Order_Item_Should_Fail(string testCaseName, Product product, ProductUnit productUnit, int quantity )
    {
        var test = () => new OrderItem(product, quantity, productUnit);

        test.Should().Throw<Exception>();
    }

    [Theory]
    [MemberData(nameof(DeliveryTestData))]
    public void Create_Delivery_Should_Fail(string testCaseName, Guid deliveryManId, Order order, Address address, DeliveryStatus deliveryStatus)
    {
        var test = () => new Delivery(deliveryManId, order, address, deliveryStatus);

        test.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(DeliveryStatus.WaitToDelivery, DeliveryStatus.Assembly)]
    [InlineData(DeliveryStatus.Delivering, DeliveryStatus.TransferredDeliveryService)]
    [InlineData(DeliveryStatus.IssuedToCourier, DeliveryStatus.WaitToDelivery)]
    [InlineData(DeliveryStatus.Cancelled, DeliveryStatus.IssuedToCourier)]
    [InlineData(DeliveryStatus.Cancelled, DeliveryStatus.Delivered)]
    [InlineData(DeliveryStatus.Cancelled, DeliveryStatus.Rejected)]
    public void Set_Delivery_Status_Should_Fail(DeliveryStatus newStatus, DeliveryStatus curretnStatus)
    {
        var delivery = new Delivery(Guid.NewGuid(), new Order(Guid.NewGuid(), new List<OrderItem>()),
            new Address("street", "houseNum", "1B", "sdcdcdc", "sdvdvdsvd"), curretnStatus);
        
        var test = () => delivery.SetStatus(newStatus);

        test.Should().Throw<Exception>();
    }
    
    [Theory]
    [InlineData("", "qds", "sd", "sdcsghcgd", "dscdsjcsd")]
    [InlineData("qwertyuiop[asdfghjkl;zxcvbnm,qwertyuiop[sdfghjkhgvhjcxfxdhghjklkdfghjkl;kjhgfghjkl;kjhgvbkl;/kjhgcgvj", "qds", "sd", "sdcsghcgd", "dscdsjcsd")]
    [InlineData("csdcsdcsdc", "", "sd", "sdcsghcgd", "dscdsjcsd")]
    [InlineData("csdcsdcsdc", "scdscdscsdcsdcsdcsdcsdcsdcs", "sd", "sdcsghcgd", "dscdsjcsd")]
    [InlineData("csdcsdcsdc", "dd", "qwertyop[asxsxsx", "sdcsghcgd", "dscdsjcsd")]
    [InlineData("csdcsdcsdc", "dd", "sd", "", "dscdsjcsd")]
    [InlineData("csdcsdcsdc", "dd", "sd", "qqqqqqqqqqaaaaaaaaaaazzzzzzzzzzzsssssssssss", "dscdsjcsd")]
    [InlineData("csdcsdcsdc", "dd", "qwertyop[asxsxsx", "sdcsghcgd", "")]
    [InlineData("csdcsdcsdc", "dd", "qwertyop[asxsxsx", "sdcsghcgd", "dscdsjcsddccsdcsdcsdcsdcsdcsdcsccdscsdcsdcsdcsdcsdcsdcsdcsdcs")]
    public void Create_Address_Should_Fail(string street, string houseNumber, string aptNumber, string city, string state)
    {
        var test = () => new Address(street, houseNumber, aptNumber, city, state);

        test.Should().Throw<Exception>();
    }

    [Theory]
    [MemberData(nameof(BillTestData))]
    public void Create_Bill_ShouldFail(Order order, DateTime creationDate, DateTime paymentDate, BillStatus status)
    {
        var test = () => new Bill(order, creationDate, paymentDate, status);

        test.Should().Throw<Exception>();
    }
}