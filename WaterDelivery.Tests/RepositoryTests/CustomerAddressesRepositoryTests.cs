using FluentAssertions;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

namespace WaterDelivery.Tests.RepositoryTests;

//TODO fail methods for all exceptions in repository
public class CustomerAddressesRepositoryTests:BaseTest
{
    private readonly WaterDeliveryContext _context;
    private readonly ICustomerAddressesRepository _customerAddressesRepository;
    private readonly IMongoCollection<CustomerAddressesDb> _customerAddresses;

    private readonly string _dbName = $"customerAddressesTest_{Guid.NewGuid()}";

    public CustomerAddressesRepositoryTests()
    {
        _context = new WaterDeliveryContext("mongodb://root:password@localhost:27017", _dbName);
        _customerAddresses = _context.GetCollection<CustomerAddressesDb>("addresses");
        _customerAddressesRepository = new CustomerAddressesRepository(_context);
    }

    [Fact]
    public async Task Create_CustomerAddress_Async_Should_Success()
    {
        var customerAddresses = new CustomerAddresses(customerId: Guid.NewGuid(), new List<Address>());
        var resultId =
            await _customerAddressesRepository.CreateCustomerAddressesAsync(customerAddresses, CancellationToken.None);

        resultId.Should().Be(customerAddresses.Id);
    }

    [Fact]
    public async Task Update_CustomerAddress_Async_Should_Success()
    {
        var customerAddresses = new CustomerAddresses(customerId: Guid.NewGuid(), new List<Address>());
        var resultId =
            await _customerAddressesRepository.CreateCustomerAddressesAsync(customerAddresses, CancellationToken.None);


        var customerAddressesNew = new CustomerAddresses(resultId,Guid.NewGuid(), new List<Address>());

        await _customerAddressesRepository.UpdateCustomerAddressesAsync(customerAddressesNew, CancellationToken.None);
        var customerAddressesDb =
            await _customerAddressesRepository.GetCustomerAddressesByIdAsync(resultId, CancellationToken.None);

        customerAddressesDb.CustomerId.Should().Be(customerAddressesNew.CustomerId);
        customerAddressesDb.Id.Should().Be(customerAddressesNew.Id);
    }

    [Fact]
    public async Task Get_CustomerAddress_By_Id_Async_ShouldSuccess()
    {
        var customerAddresses = new CustomerAddresses(customerId: Guid.NewGuid(), new List<Address>());
        var resultId =
            await _customerAddressesRepository.CreateCustomerAddressesAsync(customerAddresses, CancellationToken.None);

        var customerAddressesById = await _customerAddressesRepository.GetCustomerAddressesByIdAsync(resultId, CancellationToken.None);

        customerAddressesById.CustomerId.Should().Be(customerAddresses.CustomerId);
        customerAddressesById.Id.Should().Be(customerAddresses.Id);
    }

    [Fact]
    public async Task Get_All_Customer_Addresses_ShouldSuccess()
    {
        var customerAddress = new CustomerAddresses(customerId: Guid.NewGuid(), new List<Address>());
        var resultId =
            await _customerAddressesRepository.CreateCustomerAddressesAsync(customerAddress, CancellationToken.None);
        
        var customerAddresses = await _customerAddressesRepository.GetAllCustomerAddresses(customerAddress.CustomerId, CancellationToken.None);

        customerAddresses.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Delete_CustomerAddress_Should_Success()
    {
        var customerAddresses = new CustomerAddresses(customerId: Guid.NewGuid(), new List<Address>());
        var resultId =
            await _customerAddressesRepository.CreateCustomerAddressesAsync(customerAddresses, CancellationToken.None);

        await _customerAddressesRepository.DeleteCustomerAddressesAsync(resultId, CancellationToken.None);

        (await _customerAddresses.Find(u => u.Id == resultId).FirstOrDefaultAsync(CancellationToken.None)).Should().BeNull();
    }
    
    public override void Dispose()
    {
        _context.DeleteDb(_dbName);
    }
}