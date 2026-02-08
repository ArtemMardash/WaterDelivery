using FluentAssertions;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

namespace WaterDelivery.Tests.RepositoryTests;

//TODO fail methods for all exceptions in repository
public class BillRepositoryTests: BaseTest
{
    private readonly WaterDeliveryContext _context;
    private readonly IBillRepository _billRepository;
    private readonly IMongoCollection<BillDb> _bills;

    private readonly string _dbName = $"billTest_{Guid.NewGuid()}";

    public BillRepositoryTests()
    {
        _context = new WaterDeliveryContext("mongodb://root:password@localhost:27017/?authSource=admin", _dbName);
        _bills = _context.GetCollection<BillDb>("bills");
        _billRepository = new BillRepository(_context);
    }

    [Fact]
    public async Task Create_Bill_Async_Should_Success()
    {
        var bill = new Bill(new Order(customerId: Guid.NewGuid(), orderItems: new List<OrderItem>()), DateTime.Now.AddMinutes(10),
            DateTime.Now.AddDays(3), BillStatus.WaitForPayment);
        var resultId = await _billRepository.CreateBillAsync(bill, CancellationToken.None);

        resultId.Should().Be(bill.Id);
    }

    [Fact]
    public async Task Update_Bill_Async_Should_Success()
    {
        var bill = new Bill(new Order(customerId: Guid.NewGuid(), orderItems: new List<OrderItem>()), DateTime.UtcNow.AddMinutes(10),
            DateTime.UtcNow.AddDays(3), BillStatus.WaitForPayment);
        var resultId = await _billRepository.CreateBillAsync(bill, CancellationToken.None);


        var billNew = new Bill(resultId, new Order(customerId: Guid.NewGuid(), orderItems: new List<OrderItem>()), DateTime.UtcNow.AddDays(-2),
            DateTime.UtcNow, BillStatus.Paid);

        await _billRepository.UpdateBillAsync(billNew, CancellationToken.None);
        var billDb = await _billRepository.GetBillByIdAsync(resultId, CancellationToken.None);

        billDb.CreationDate.Should().BeSameDateAs(billNew.CreationDate);
        billDb.Status.Should().Be(billNew.Status);
        billDb.PaymentDate.Should().BeSameDateAs(billNew.PaymentDate.Value);
        billDb.Id.Should().Be(billNew.Id);
    }

    [Fact]
    public async Task Get_Bill_By_Id_Async_ShouldSuccess()
    {
        var bill = new Bill(new Order(customerId: Guid.NewGuid(), orderItems: new List<OrderItem>()), DateTime.UtcNow.AddMinutes(10),
            DateTime.UtcNow.AddDays(3), BillStatus.WaitForPayment);
        var resultId = await _billRepository.CreateBillAsync(bill, CancellationToken.None);

        var billById = await _billRepository.GetBillByIdAsync(resultId, CancellationToken.None);


        billById.CreationDate.Should().BeSameDateAs(bill.CreationDate);
        billById.Status.Should().Be(bill.Status);
        billById.PaymentDate.Should().NotBeNull();
        billById.PaymentDate.Should().BeSameDateAs(bill.PaymentDate.Value);
        billById.Id.Should().Be(bill.Id);
    }
    
    [Fact]
    public async Task Delete_Bill_Should_Success()
    {
        var bill = new Bill(new Order(customerId: Guid.NewGuid(), orderItems: new List<OrderItem>()), DateTime.Now.AddMinutes(10),
            DateTime.Now.AddDays(3), BillStatus.WaitForPayment);
        var resultId = await _billRepository.CreateBillAsync(bill, CancellationToken.None);

        await _billRepository.DeleteAsync(resultId, CancellationToken.None);

        (await _bills.Find(u => u.Id == resultId).FirstOrDefaultAsync(CancellationToken.None)).Should().BeNull();
    }
    
    public override void Dispose()
    {
        _context.DeleteDb(_dbName);
    }
}