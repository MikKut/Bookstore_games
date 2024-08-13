using AutoMapper;
using BG.LocalWeb.Application.Common.DTOs.User;
using BG.LocalWeb.Application.Handlers.Users.Commands;
using BG.LocalWeb.Domain.Entities;
using BG.LocalWeb.Domain.Interfaces.Repositories;
using BG.LocalWeb.Domain.Interfaces.Services;
using BG.LocalWeb.Domain.Specifications.UserSpecifications;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalWeb.Handlers.Commands
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _jwtTokenServiceMock = new Mock<IJwtTokenService>();
            _mapperMock = new Mock<IMapper>();

            _handler = new LoginUserCommandHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtTokenServiceMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ShouldReturnFailure()
        {
            // Arrange
            var command = new LoginUserCommand { Username = "nonexistent", Password = "password" };
            _userRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<UserByUsernameSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains("Invalid username or password.", result.Errors);
        }

        [Fact]
        public async Task Handle_InvalidPassword_ShouldReturnFailure()
        {
            // Arrange
            var command = new LoginUserCommand { Username = "user", Password = "wrongpassword" };
            var user = new User { UserName = "user", PasswordHash = "hashedpassword" };

            _userRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<UserByUsernameSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordHasherMock.Setup(hasher => hasher.VerifyHashedPassword(user, "hashedpassword", "wrongpassword"))
                .Returns(PasswordVerificationResult.Failed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains("Invalid username or password.", result.Errors);
        }

        [Fact]
        public async Task Handle_SuccessfulLogin_ShouldReturnSuccessWithToken()
        {
            // Arrange
            var command = new LoginUserCommand { Username = "user", Password = "correctpassword" };
            var user = new User { UserName = "user", PasswordHash = "hashedpassword" };
            var token = "generatedToken";
            var userDto = new UserDto { Username = "user" };

            _userRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<UserByUsernameSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordHasherMock.Setup(hasher => hasher.VerifyHashedPassword(user, "hashedpassword", "correctpassword"))
                .Returns(PasswordVerificationResult.Success);

            _jwtTokenServiceMock.Setup(service => service.GenerateToken(user))
                .Returns(token);

            _mapperMock.Setup(mapper => mapper.Map<UserDto>(user))
                .Returns(userDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Equal(token, result.Data.Token);
            Assert.Equal(userDto.Username, result.Data.User.Username);
        }

        [Fact]
        public async Task Handle_SuccessRehashNeeded_ShouldRehashPasswordAndReturnSuccess()
        {
            // Arrange
            var command = new LoginUserCommand { Username = "user", Password = "correctpassword" };
            var user = new User { UserName = "user", PasswordHash = "hashedpassword" };
            var token = "generatedToken";
            var userDto = new UserDto { Username = "user" };

            _userRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<UserByUsernameSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordHasherMock.SetupSequence(hasher => hasher.VerifyHashedPassword(user, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PasswordVerificationResult.SuccessRehashNeeded)
                .Returns(PasswordVerificationResult.Success);

            _jwtTokenServiceMock.Setup(service => service.GenerateToken(user))
                .Returns(token);

            _mapperMock.Setup(mapper => mapper.Map<UserDto>(user))
                .Returns(userDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Equal(token, result.Data.Token);
            Assert.Equal(userDto.Username, result.Data.User.Username);
        }
    }
}
