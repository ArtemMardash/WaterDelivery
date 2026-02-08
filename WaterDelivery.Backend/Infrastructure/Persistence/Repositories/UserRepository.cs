using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<UserDb> _users;

    public UserRepository(WaterDeliveryContext context)
    {
        _users = context.GetCollection<UserDb>("users");
    }

    public async Task<Guid> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        var userDb = user.ToDb();
        await _users.InsertOneAsync(userDb, cancellationToken);
        return userDb.Id;
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        if (!await IsUserExists(user, cancellationToken))
        {
            throw new InvalidOperationException("There is no user with such Id");
        }

        var db = user.ToDb();
        var update = Builders<UserDb>.Update
            .Set(u => u.PhoneNumber, db.PhoneNumber)
            .Set(u => u.UserType, db.UserType)
            .Set(u => u.Email, db.Email)
            .Set(u => u.Name, db.Name);
        await _users.ReplaceOneAsync(u => u.Id == user.Id, user.ToDb(), cancellationToken: cancellationToken);
    }

    public async Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var userDb = await _users.Find(u => u.Id == id).FirstOrDefaultAsync(cancellationToken);

        return userDb.ToDomain();
    }

    public async Task<List<User>> GetAllWorkersAsync(CancellationToken cancellationToken)
    {
        var result = await _users.Find(u => u.UserType == 2).ToListAsync(cancellationToken);

        return result.Select(u => u.ToDomain()).ToList();
    }

    public async Task<List<User>> GetAllCustomersAsync(CancellationToken cancellationToken)
    {
        var result = await _users.Find(u => u.UserType == 1).ToListAsync(cancellationToken);

        return result.Select(u => u.ToDomain()).ToList();
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken)
    {
        await _users.DeleteOneAsync(u => u.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<bool> IsUserExists(User user, CancellationToken cancellationToken)
    {
        if (await _users.Find(u => u.Id == user.Id).FirstOrDefaultAsync(cancellationToken) == null &&
            await _users.Find(u => u.Email == user.Email).FirstOrDefaultAsync(cancellationToken) == null &&
            await _users.Find(u => u.PhoneNumber == user.PhoneNumber).FirstOrDefaultAsync(cancellationToken) == null)
        {
            return false;
        }

        return true;
    }
}