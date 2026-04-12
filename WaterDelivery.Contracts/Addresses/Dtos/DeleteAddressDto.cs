using Mediator;

namespace WaterDelivery.Contracts.Addresses.Dtos;

public class DeleteAddressDto: IRequest
{
    public Guid Id { get; set; }

}