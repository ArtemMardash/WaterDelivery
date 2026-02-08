using Mediator;

namespace WaterDelivery.Backend.Features.Addresses.Dtos;

public class UpdateAddressDto: IRequest
{
    public Guid Id { get; set; }

    public string Street { get; set; }

    public string HouseNumber { get; set; }

    public string? AptNumber { get; set; }

    public string City { get; set; }

    public string State { get; set; }
    
    public bool isDeleted { get; set; }
}