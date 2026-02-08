using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Features.Users.Dtos;

namespace WaterDelivery.Backend.Features.Users;

public class CreateUserUseCase: IRequestHandler<CreateUserDto, Guid>
{
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Constructor to save and create user
    /// </summary>
    public CreateUserUseCase(IUserRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    /// <summary>
    /// Method to create and add a new user
    /// </summary>
    public async ValueTask<Guid> Handle(CreateUserDto dto, CancellationToken cancellationToken)
    {
        var user = new User(dto.Name, dto.UserType, dto.Email, dto.PhoneNumber);
        if (await _repository.IsUserExists(user, cancellationToken))
        {
            throw new InvalidOperationException("User with such Email or Phone number already exists");
        }
        await  _repository.CreateUserAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}