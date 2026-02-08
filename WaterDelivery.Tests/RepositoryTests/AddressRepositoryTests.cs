using FluentAssertions;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

namespace WaterDelivery.Tests.RepositoryTests;

//TODO fail methods for all exceptions in repository
public class AddressRepositoryTests: BaseTest
{
    private readonly WaterDeliveryContext _context;
    private readonly IAddressRepository _addressRepository;
    private readonly IMongoCollection<AddressDb> _address;

    private readonly string _dbName = $"addressTest_{Guid.NewGuid()}";

    public AddressRepositoryTests()
    {
        _context = new WaterDeliveryContext("mongodb://root:password@localhost:27017", _dbName);
        _address = _context.GetCollection<AddressDb>("address");
        _addressRepository = new AddressRepository(_context);
    }

    [Fact]
    public async Task Create_Address_Async_Should_Success()
    {
        var address = new Address("street", "hr12", "2b", "City", "New York");
        var resultId = await _addressRepository.CreateAddressAsync(address, CancellationToken.None);

        resultId.Should().Be(address.Id);
    }

    [Fact]
    public async Task Update_Address_Async_Should_Success()
    {
        var address = new Address("street", "hr12", "2b", "City", "New York");

        var resultId = await _addressRepository.CreateAddressAsync(address, CancellationToken.None);


        var addressNew = new Address(resultId, "street1", "hr121", "2b1", "City1", "New York1", false);

        await _addressRepository.UpdateAddressAsync(addressNew, CancellationToken.None);
        var addressDb = await _address.Find(u => u.Id == resultId).FirstOrDefaultAsync(CancellationToken.None);

        addressDb.Street.Should().Be(addressNew.Street);
        addressDb.HouseNumber.Should().Be(addressNew.HouseNumber);
        addressDb.AptNumber.Should().Be(addressNew.AptNumber);
        addressDb.City.Should().Be(addressNew.City);
        addressDb.State.Should().Be(addressNew.State);
        addressDb.Id.Should().Be(addressNew.Id);
    }

    [Fact]
    public async Task Get_Address_By_Id_Async_ShouldSuccess()
    {
        var address = new Address("street", "hr12", "2b", "City", "New York");

        var resultId = await _addressRepository.CreateAddressAsync(address, CancellationToken.None);

        var addressById = await _addressRepository.GetAddressAsync(resultId, CancellationToken.None);

        addressById.Street.Should().Be(address.Street);
        addressById.HouseNumber.Should().Be(address.HouseNumber);
        addressById.AptNumber.Should().Be(address.AptNumber);
        addressById.City.Should().Be(address.City);
        addressById.Id.Should().Be(address.Id);
        addressById.State.Should().Be(address.State);
    }
    
    [Fact]
    public async Task Delete_Address_Should_Success()
    {
        var address = new Address("street", "hr12", "2b", "City", "New York");
        var resultId = await _addressRepository.CreateAddressAsync(address, CancellationToken.None);

        await _addressRepository.DeleteAsync(resultId, CancellationToken.None);

        (await _address.Find(u => u.Id == resultId && u.isDeleted == false).FirstOrDefaultAsync(CancellationToken.None)).Should().BeNull();
    }
    
    public override void Dispose()
    {
        _context.DeleteDb(_dbName);
    }
}