using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Author;
using BG.LocalApi.Application.Handlers.Authors.Commands;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalApi.Domain.Specifications.AuthorSpecificaitons;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalApi.Tests.HandlersTest.Authors.Commands
{
    public class CreateAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorRepository> _mockAuthorRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateAuthorCommandHandler _handler;

        public CreateAuthorCommandHandlerTests()
        {
            _mockAuthorRepository = new Mock<IAuthorRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreateAuthorCommandHandler(_mockAuthorRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenAuthorIsCreated()
        {
            // Arrange
            var command = new CreateAuthorCommand
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1)
            };
            var author = new Author
            {
                Id = Guid.NewGuid(),
                FirstName = command.FirstName,
                LastName = command.LastName,
                DateOfBirth = command.DateOfBirth
            };
            var authorDto = new AuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                DateOfBirth = author.DateOfBirth
            };

            _mockMapper.Setup(m => m.Map<Author>(command)).Returns(author);
            _mockAuthorRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<AuthorByFirstLastNameAndBirthDateSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync((Author)null!);
            _mockAuthorRepository.Setup(r => r.AddAsync(author, It.IsAny<CancellationToken>())).ReturnsAsync(author);
            _mockMapper.Setup(m => m.Map<AuthorDto>(author)).Returns(authorDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(authorDto, result.Data);
            _mockAuthorRepository.Verify(r => r.AddAsync(author, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenAuthorAlreadyExists()
        {
            // Arrange
            var command = new CreateAuthorCommand
            {
                FirstName = "Jane",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 2, 15)
            };
            var existingAuthor = new Author
            {
                Id = Guid.NewGuid(),
                FirstName = command.FirstName,
                LastName = command.LastName,
                DateOfBirth = command.DateOfBirth
            };

            _mockMapper.Setup(m => m.Map<Author>(command)).Returns(existingAuthor);
            _mockAuthorRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<AuthorByFirstLastNameAndBirthDateSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingAuthor);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains($"Cannot create {command.LastName} {command.FirstName}: the author with such name and date birth already exists", result.Errors);
            _mockAuthorRepository.Verify(r => r.AddAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

}
