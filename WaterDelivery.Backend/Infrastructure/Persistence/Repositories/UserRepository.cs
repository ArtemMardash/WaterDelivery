using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

public class UserRepository: IUserRepository
{
    private readonly IMongoCollection<UserDb> _users;

    public UserRepository(WaterDeliveryContext context)
    {
        _users = context.GetCollection<UserDb>("user");
    }
    
    public async Task<string> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        var userDb = user.ToDb();
        await _users.InsertOneAsync(userDb, cancellationToken);
        return userDb.Id;
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        await _users.ReplaceOneAsync(user.Id.ToString(), user.ToDb(), cancellationToken: cancellationToken);
    }

    public async Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var userDb = await _users.Find(u=>u.Id == id.ToString()).FirstOrDefaultAsync(cancellationToken);

        return userDb.ToDomain();
    }

    public async Task<List<User>> GetAllWorkersAsync(CancellationToken cancellationToken)
    {
        var result = await _users.Find(u=>u.UserType == 2).ToListAsync(cancellationToken);

        return result.Select(u => u.ToDomain()).ToList();
    }

    public async Task<List<User>> GetAllCustomersAsync(CancellationToken cancellationToken)
    {
        var result = await _users.Find(u=>u.UserType == 1).ToListAsync(cancellationToken);

        return result.Select(u => u.ToDomain()).ToList();
    }

    public async Task DeleteUserAsync(Guid id,CancellationToken cancellationToken)
    {
        await _users.DeleteOneAsync(u => u.Id == id.ToString(), cancellationToken: cancellationToken);
    }
}