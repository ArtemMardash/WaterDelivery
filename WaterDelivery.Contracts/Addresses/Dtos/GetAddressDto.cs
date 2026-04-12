using Mediator;

namespace WaterDelivery.Contracts.Addresses.Dtos;

public class GetAddressDto: IRequest<GetAddressResultDto>
{
    public Guid Id { get; set; }
}