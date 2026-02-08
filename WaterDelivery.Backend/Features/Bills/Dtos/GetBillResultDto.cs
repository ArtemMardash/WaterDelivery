using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Features.Orders.Dtos;

namespace WaterDelivery.Backend.Features.Bills.Dtos;

public class GetBillResultDto
{
    public Guid Id { get; set; }
    
    public OrderDto Order { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public DateTime? PaymentDate { get; set; }
    
    public BillStatus Status { get; set; }
}