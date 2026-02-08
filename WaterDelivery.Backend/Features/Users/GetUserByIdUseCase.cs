using Mediator;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Features.Users.Dtos;

namespace WaterDelivery.Backend.Features.Users;

public class GetUserByIdUseCase: IRequestHandler<GetUserDto, GetUserResultDto>
{
    private readonly IUserRepository _repository;

    /// <summary>
    /// Constructor to save and create user
    /// </summary>
    public GetUserByIdUseCase(IUserRepository repository)
    {
        _repository = repository;
    }
    
    public async ValueTask<GetUserResultDto> Handle(GetUserDto request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserByIdAsync(request.Id, cancellationToken);

        return new GetUserResultDto
        {
            Id = user.Id,
            Name = user.Name,
            UserType = user.UserType,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }
}