using FluentAssertions;
using Mediator;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Features.Users.Dtos;
using WaterDelivery.Backend.Infrastructure.Persistence;

namespace WaterDelivery.Tests.IntegrationTests;

public class UserUseCases: IDisposable
{
    private readonly IUserRepository _userRepository;
    private readonly WaterDeliveryContext _waterDeliveryContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IntegrationTestHelper _dbService = new IntegrationTestHelper();
    private static Guid _userId;

    public UserUseCases()
    {
        _mediator = _dbService.GetRequiredService<IMediator>();
        _userRepository = _dbService.GetRequiredService<IUserRepository>();
        _unitOfWork = _dbService.GetRequiredService<IUnitOfWork>();
        _userId = _dbService.CreateUser(_userRepository, _unitOfWork);
        _waterDeliveryContext = _dbService.GetRequiredService<WaterDeliveryContext>();
    }

    [Fact]
    public async Task Create_User_Use_Case_Should_Success()
    {
        var request = new CreateUserDto
        {
            Name = "Artem1",
            UserType = UserType.Customer,
            Email = "artem.m3@gmail.com",
            PhoneNumber = "1234567890"
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        result.Should().NotBe(Guid.Empty);
    }
    
    [Fact]
    public async Task Update_User_Use_Case_Should_Success()
    {
        var request1 = new CreateUserDto
        {
            Name = "Artem2",
            UserType = UserType.Customer,
            Email = "artem.m4@gmail.com",
            PhoneNumber = "1234567891"
        };

        var result = await _mediator.Send(request1, CancellationToken.None);
        
        var request = new UpdateUserDto
        {
            Id = result,
            Name = "Artem2",
            UserType = UserType.Customer,
            Email = "arte.m5@gmail.com",
            PhoneNumber = "1234567890"
        };

        await _mediator.Send(request, CancellationToken.None);

        var user = await _waterDeliveryContext
            .GetCollection<User>("users")
            .Find(u => u.Id == result)
            .FirstOrDefaultAsync();

        user.Email.Should().Be("arte.m5@gmail.com");
        user.UserType.Should().Be(UserType.Customer);
        user.PhoneNumber.Should().Be("1234567890");
        user.Name.Should().Be("Artem2");
    }

    [Fact]
    public async Task Delete_User_Use_Case_Should_Success()
    {
        var request = new DeleteUserDto
        {
            Id = _userId
        };

        await _mediator.Send(request, CancellationToken.None);
        var exists = await _waterDeliveryContext.GetCollection<User>("users").Find(u => u.Id == _userId).AnyAsync();

        exists.Should().BeFalse();
    }

    [Fact]
    public async Task Get_User_Use_Case_Should_Success()
    {
        var request = new GetUserDto
        {
            Id = _userId
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        result.Email.Should().Be("artem.m@gmail.com");
        result.UserType.Should().Be(UserType.Customer);
        result.Name.Should().Be("Artem");
        result.PhoneNumber.Should().Be("9296996892");
        result.Id.Should().Be(_userId);
    }



    public void Dispose()
    {
        _dbService.DeleteDb(_waterDeliveryContext);
        _unitOfWork.Dispose();
    }
}