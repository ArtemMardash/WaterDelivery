using Mediator;

namespace WaterDelivery.Backend.Features.Addresses.Dtos;

public class DeleteAddressDto: IRequest
{
    public Guid Id { get; set; }

}