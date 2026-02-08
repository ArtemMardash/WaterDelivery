using Mediator;

namespace WaterDelivery.Backend.Features.Addresses.Dtos;

public class GetAddressDto: IRequest<GetAddressResultDto>
{
    public Guid Id { get; set; }
}