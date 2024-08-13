using Ardalis.Specification;
using AutoMapper;
using BG.LocalWeb.Application.Common.DTOs.User;
using BG.LocalWeb.Application.Handlers.Users.Commands;
using BG.LocalWeb.Domain.Entities;
using BG.LocalWeb.Domain.Interfaces.Repositories;
using BG.LocalWeb.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalWeb.Handlers.Commands
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _jwtTokenServiceMock = new Mock<IJwtTokenService>();
            _mapperMock = new Mock<IMapper>();
            _handler = new RegisterUserCommandHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtTokenServiceMock.Object,
                _mapperMock.Object
            );
        }
        [Fact]
        public async Task Handle_ShouldRegisterUserSuccessfully_WhenUsernameIsAvailable()
        {
            // Arrange
            var request = new RegisterUserCommand(new UserCreateDto
            {
                Username = "newuser",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Street"
            });

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "newuser",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Street"
            };

            _userRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            _passwordHasherMock.Setup(ph => ph.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns("hashedPassword");

            _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _jwtTokenServiceMock.Setup(jwt => jwt.GenerateToken(It.IsAny<User>()))
                .Returns("test-token");

            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                .Returns(new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    Address = user.Address
                });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("test-token", result.Data.Token);
            Assert.Equal(user.Id, result.Data.User.Id);
            Assert.Equal(user.UserName, result.Data.User.Username);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUsernameIsTaken()
        {
            // Arrange
            var existingUser = new User { UserName = "existinguser" };

            _userRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            var request = new RegisterUserCommand(new UserCreateDto { Username = "existinguser" });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Username is already taken.", result.Errors.First());
        }

        [Fact]
        public async Task Handle_ShouldGenerateToken_WhenUserIsRegistered()
        {
            // Arrange
            var request = new RegisterUserCommand(new UserCreateDto
            {
                Username = "newuser",
                Password = "password123"
            });

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = "newuser",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Street"
            };

            var userDto = new UserDto
            {
                Id = newUser.Id,
                Username = newUser.UserName,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                DateOfBirth = newUser.DateOfBirth,
                Address = newUser.Address
            };

            _userRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            _passwordHasherMock.Setup(ph => ph.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns("hashedPassword");

            _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(newUser);

            _jwtTokenServiceMock.Setup(jwt => jwt.GenerateToken(newUser))
                .Returns("test-token");

            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                .Returns(userDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _jwtTokenServiceMock.Verify(jwt => jwt.GenerateToken(It.Is<User>(u => u.Id == newUser.Id)), Times.Once);
            Assert.Equal("test-token", result.Data.Token);
        }
    }
}