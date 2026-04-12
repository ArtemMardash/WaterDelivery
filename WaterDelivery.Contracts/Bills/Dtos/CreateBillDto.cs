using Mediator;
using WaterDelivery.Contracts.Enums;
using WaterDelivery.Contracts.Orders.Dtos;

namespace WaterDelivery.Contracts.Bills.Dtos;

public class CreateBillDto: IRequest<Guid>
{
    public OrderDto Order { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public DateTime? PaymentDate { get; set; }
    
    public BillStatus Status { get; set; }
}