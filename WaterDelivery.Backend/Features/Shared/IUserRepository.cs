using WaterDelivery.Backend.Core.Entities;

namespace WaterDelivery.Backend.Features.Shared;

public interface IUserRepository
{
    public Task<string> CreateUserAsync(User user, CancellationToken cancellationToken);

    public Task UpdateUserAsync(User user, CancellationToken cancellationToken);

    public Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<List<User>> GetAllWorkersAsync(CancellationToken cancellationToken);

    public Task<List<User>> GetAllCustomersAsync(CancellationToken cancellationToken);

    public Task DeleteUserAsync(Guid id,CancellationToken cancellationToken);
}