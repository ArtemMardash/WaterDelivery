using Mediator;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Features.Users.Dtos;

namespace WaterDelivery.Backend.Features.Users;

public class DeleteUserUseCase: IRequestHandler<DeleteUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Unit> Handle(DeleteUserDto request, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteUserAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}