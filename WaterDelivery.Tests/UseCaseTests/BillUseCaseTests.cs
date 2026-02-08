using FluentAssertions;
using NSubstitute;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Bills;
using WaterDelivery.Backend.Features.Bills.Dtos;
using WaterDelivery.Backend.Features.Orders.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Tests.UseCaseTests;

public class BillUseCaseTests
{
    private readonly IBillRepository _billRepository;

    private readonly IUnitOfWork _unitOfWork;
    
    public BillUseCaseTests()
    {
        _billRepository = Substitute.For<IBillRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task Create_Bill_Should_Success()
    {
        var dto = new CreateBillDto
        {
            Order = new OrderDto
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Items = new List<OrderItemDto>()
            },
            CreationDate = DateTime.Now.AddMinutes(3),
            PaymentDate = DateTime.Now.AddDays(3),
            Status = BillStatus.WaitForPayment
        };

        var createBillUseCase = new CreateBillUseCase(_billRepository, _unitOfWork);
        var result = await createBillUseCase.Handle(dto, CancellationToken.None);

        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Get_Bill_Should_Success()
    {
        var bill = new Bill(
            new Order(Guid.NewGuid(), Guid.NewGuid(), new List<OrderItem>()), 
            DateTime.Now.AddMinutes(3), 
            DateTime.Now.AddDays(3), 
            BillStatus.WaitForPayment);

        _billRepository.GetBillByIdAsync(Arg.Any<Guid>(), CancellationToken.None).Returns(bill);

        var useCase = new GetBillUseCase(_billRepository);
        var result = await useCase.Handle(new GetBillDto
        {
            Id = bill.Id
        }, CancellationToken.None);

        result.Id.Should().Be(bill.Id);
        result.Order.Id.Should().Be(bill.Order.Id);
        result.Order.CustomerId.Should().Be(bill.Order.CustomerId);
        result.CreationDate.Should().Be(bill.CreationDate);
        result.Status.Should().Be(bill.Status);
        result.PaymentDate.Should().Be(bill.PaymentDate);
    }
}