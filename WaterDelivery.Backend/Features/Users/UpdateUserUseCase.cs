using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Features.Users.Dtos;

namespace WaterDelivery.Backend.Features.Users;

public class UpdateUserUseCase: IRequestHandler<UpdateUserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _repository;

    /// <summary>
    /// Constructor to edit and save data
    /// </summary>
    public UpdateUserUseCase(IUserRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Unit> Handle(UpdateUserDto request, CancellationToken cancellationToken)
    {
        var userToUpdate = new User(request.Id, request.Name, request.UserType ,request.Email, request.PhoneNumber);
        
        await _repository.UpdateUserAsync(userToUpdate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}