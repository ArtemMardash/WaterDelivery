using Bogus;
using FluentAssertions;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

namespace WaterDelivery.Tests.RepositoryTests;

//TODO fail methods for all exceptions in repository
public class UserRepositoryTests : BaseTest
{
    private readonly WaterDeliveryContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IMongoCollection<UserDb> _user;

    private readonly string _dbName = $"usersTest_{Guid.NewGuid()}";

    public UserRepositoryTests()
    {
        _context = new WaterDeliveryContext("mongodb://root:password@localhost:27017", _dbName);
        _user = _context.GetCollection<UserDb>("users");
        _userRepository = new UserRepository(_context);
        
    }
    

    [Fact]
    public async Task Create_User_Async_Should_Success()
    {
        var user = new User("12345", UserType.Customer, "artem.m3@gmail.com", "1231231245");
        var resultId = await _userRepository.CreateUserAsync(user, CancellationToken.None);

        resultId.Should().Be(user.Id);
    }

    [Fact]
    public async Task Update_User_Async_Should_Success()
    {
        var user = new User("12345", UserType.Customer, "artem.m3@gmail.com", "1231231245");
        var resultId = await _userRepository.CreateUserAsync(user, CancellationToken.None);

        var newUser = new User(resultId, "12346", UserType.Customer, "artem.m4@gmail.com", "1241241245");
        await _userRepository.UpdateUserAsync(newUser, CancellationToken.None);
        var userDb = await _user.Find(u => u.Id == resultId).FirstOrDefaultAsync(CancellationToken.None);

        userDb.PhoneNumber.Should().Be(newUser.PhoneNumber);
        userDb.Name.Should().Be(newUser.Name);
        userDb.Email.Should().Be(newUser.Email);
        userDb.UserType.Should().Be((int)newUser.UserType);
        userDb.Id.Should().Be(newUser.Id);
    }

    [Fact]
    public async Task Get_User_By_Id_Async_ShouldSuccess()
    {
        var user = new User("12345", UserType.Customer, "artem.m3@gmail.com", "1231231245");
        var resultId = await _userRepository.CreateUserAsync(user, CancellationToken.None);

        var userById = await _userRepository.GetUserByIdAsync(resultId, CancellationToken.None);

        userById.UserType.Should().Be(user.UserType);
        userById.Email.Should().Be(user.Email);
        userById.PhoneNumber.Should().Be(user.PhoneNumber);
        userById.Name.Should().Be(user.Name);
        userById.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task Get_All_Workers_Async_Should_Success()
    {
        var faker = new Faker("en");
        Random rand = new Random();

        for (var i = 0; i < 10; i++)
        {
            var user = new User(
                faker.Name.FullName(),
                (UserType)rand.Next(1, 2),
                faker.Internet.Email(),
                faker.Phone.PhoneNumberFormat()
            );

            await _userRepository.CreateUserAsync(user, CancellationToken.None);
        }

        var result = await _userRepository.GetAllWorkersAsync(CancellationToken.None);

        foreach (var u in result)
        {
            u.UserType.Should().Be(UserType.DeliveryMen);
        }
    }

    [Fact]
    public async Task Get_All_Customers_Async_Should_Success()
    {
        var faker = new Faker("en");
        Random rand = new Random();

        for (var i = 0; i < 10; i++)
        {
            var user = new User(
                faker.Name.FullName(),
                (UserType)rand.Next(1, 2),
                faker.Internet.Email(),
                faker.Phone.PhoneNumberFormat()
            );

            await _userRepository.CreateUserAsync(user, CancellationToken.None);
        }

        var result = await _userRepository.GetAllCustomersAsync(CancellationToken.None);

        foreach (var u in result)
        {
            u.UserType.Should().Be(UserType.Customer);
        }
    }

    [Fact]
    public async Task Delete_User_Should_Success()
    {
        var user = new User("12345", UserType.Customer, "artem.m3@gmail.com", "1231231245");
        var resultId = await _userRepository.CreateUserAsync(user, CancellationToken.None);

        await _userRepository.DeleteUserAsync(resultId, CancellationToken.None);

        (await _user.Find(u => u.Id == resultId).FirstOrDefaultAsync(CancellationToken.None)).Should().BeNull();
    }

    [Theory]
    [InlineData("12123", UserType.Customer, "artem.m5@gmail.com", "1212121212", false)]
    [InlineData("12123", UserType.Customer, "artem.m6@gmail.com", "1212121212", true)]
    [InlineData("12123", UserType.Customer, "artem.m9@gmail.com", "1029384757", true)]
    [InlineData("12123", UserType.Customer, "artem.m8@gmail.com", "1029384758", true)]
    public async Task Is_User_Exists_Async_Should_Success(string name, UserType userType, string email, string phoneNumber, bool result)
    {
        await PrepareDbAsync();

        var user = new User(name, userType, email, phoneNumber);

        (await _userRepository.IsUserExists(user, CancellationToken.None)).Should().Be(result);
    }

    private async Task PrepareDbAsync()
    {
        await _userRepository.CreateUserAsync(new User("artem1", UserType.Customer, "artem.m6@gmail.com", "1029384756"),
            CancellationToken.None);
        await _userRepository.CreateUserAsync(new User("artem2", UserType.Customer, "artem.m7@gmail.com", "1029384757"),
            CancellationToken.None);
        await _userRepository.CreateUserAsync(new User("artem3", UserType.Customer, "artem.m8@gmail.com", "1029384758"),
            CancellationToken.None);
    }
    
    public override void Dispose()
    {
        _context.DeleteDb(_dbName);
    }
}