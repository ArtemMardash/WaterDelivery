using FluentAssertions;
using NSubstitute;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Features.Users;
using WaterDelivery.Backend.Features.Users.Dtos;

namespace WaterDelivery.Tests.UseCaseTests;

public class UserUseCaseTests
{
    private IUserRepository _repository;
    private IUnitOfWork _unitOfWork;
    
    public UserUseCaseTests()
    {
        _repository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
    }
    [Fact]
    public async Task Create_User_Should_Success_Async()
    {
        var dto = new CreateUserDto
        {
            Name = "Artem",
            UserType = UserType.Customer,
            Email = "artem.m1@gmail.com",
            PhoneNumber = "1234567890"
        };
        _repository.IsUserExists(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(false);
        var createUserUseCase = new CreateUserUseCase(_repository, _unitOfWork);
        var result = await createUserUseCase.Handle(dto, CancellationToken.None);
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Create_User_Should_Fail_Async()
    {
        var dto = new CreateUserDto
        {
            Name = "Artem",
            UserType = UserType.Customer,
            Email = "artem.m1@gmail.com",
            PhoneNumber = "1234567890"
        };
        _repository.IsUserExists(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(true);
        var createUserUseCase = new CreateUserUseCase(_repository, _unitOfWork);
        var test = async () => await createUserUseCase.Handle(dto, CancellationToken.None);
        await test.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("User with such Email or Phone number already exists");
    }

    [Fact]
    public async Task Get_User_Should_Success()
    {
        var user = new User("artem", UserType.Customer, "artem.m2@gmail.com", "1234567890");
        _repository.GetUserByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(user);
        var getUserUseCase = new GetUserByIdUseCase(_repository);
        var result = await getUserUseCase.Handle(new GetUserDto
        {
            Id = Guid.NewGuid()
        }, CancellationToken.None);

        result.Id.Should().Be(user.Id);
        result.PhoneNumber.Should().Be(user.PhoneNumber);
        result.Email.Should().Be(user.Email);
        result.Name.Should().Be(user.Name);
        result.UserType.Should().Be(user.UserType);
    }
}