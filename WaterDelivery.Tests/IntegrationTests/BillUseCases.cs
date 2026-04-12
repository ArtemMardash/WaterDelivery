using FluentAssertions;
using Mediator;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Bills.Dtos;
using WaterDelivery.Backend.Features.Orders.Dtos;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;

namespace WaterDelivery.Tests.IntegrationTests;

public class BillUseCases: IDisposable
{
    private readonly IBillRepository _billRepository;
    private readonly WaterDeliveryContext _waterDeliveryContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IntegrationTestHelper _dbService = new IntegrationTestHelper();
    private static Guid _billId;

    public BillUseCases()
    {
        _billRepository = _dbService.GetRequiredService<IBillRepository>();
        _mediator = _dbService.GetRequiredService<IMediator>();
        _unitOfWork = _dbService.GetRequiredService<IUnitOfWork>();
        _waterDeliveryContext = _dbService.GetRequiredService<WaterDeliveryContext>();
        _billId = _dbService.CreateBill(_billRepository, _unitOfWork);
    }

    [Fact]
    public async Task Create_Bill_Use_Case_Should_Success()
    {
        var request = new CreateBillDto
        {
            Order = new OrderDto
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Items = new List<OrderItemDto>()
            },
            CreationDate = DateTime.UtcNow.AddMinutes(30),
            PaymentDate = DateTime.UtcNow.AddDays(7),
            Status = BillStatus.WaitForPayment
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Update_Bill_Use_Case_Should_Success()
    {
        var createdId = await _mediator.Send(new CreateBillDto
        {
            Order = new OrderDto
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Items = new List<OrderItemDto>()
            },
            CreationDate = DateTime.UtcNow.AddMinutes(30),
            PaymentDate = DateTime.UtcNow.AddDays(7),
            Status = BillStatus.WaitForPayment
        }, CancellationToken.None);
        

        var update = new UpdateBillDto
        {
            Id = createdId,
            Order = new OrderDto
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Items = new List<OrderItemDto>()
            },
            CreationDate = DateTime.UtcNow.AddMinutes(700),
            PaymentDate = DateTime.UtcNow.AddDays(14),
            Status = BillStatus.Paid,
        };

        await _mediator.Send(update, CancellationToken.None);

        var bill = await _waterDeliveryContext
            .GetCollection<Bill>("bill")
            .Find(b => b.Id == createdId)
            .FirstOrDefaultAsync();

        bill.Should().NotBeNull();
        bill.Order.Should().NotBeNull();
        bill.CreationDate.Should().BeCloseTo(update.CreationDate, TimeSpan.FromSeconds(1));
        bill.PaymentDate.Should().BeCloseTo(update.PaymentDate!.Value, TimeSpan.FromSeconds(1));
        bill.Status.Should().Be(BillStatus.Paid);
    }

    [Fact]
    public async Task Delete_Bill_Use_Case_Should_Success()
    {
        await _mediator.Send(new DeleteBillDto { Id = _billId }, CancellationToken.None);

        var exists = await _waterDeliveryContext
            .GetCollection<Bill>("bill")
            .Find(b => b.Id == _billId)
            .AnyAsync();

        exists.Should().BeFalse();
    }

    [Fact]
    public async Task Get_Bill_Use_Case_Should_Success()
    {
        var result = await _mediator.Send(new GetBillDto { Id = _billId }, CancellationToken.None);

        result.Id.Should().Be(_billId);
        result.Order.Should().NotBeNull();
        result.CreationDate.Should().BeCloseTo(DateTime.UtcNow.AddDays(1), TimeSpan.FromMinutes(1));
        result.PaymentDate.Should().NotBeNull();
        result.Status.Should().Be(BillStatus.WaitForPayment);
    }

    public void Dispose()
    {
        _dbService.DeleteDb(_waterDeliveryContext);
    }
}