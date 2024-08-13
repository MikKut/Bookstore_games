using AutoMapper;
using BG.LocalApi.Application.Handlers.Authors.Commands;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalWebApp.Application.Common.Specifications.Base;
using BG.LocalWebApp.Common.Constants;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalApi.Tests.HandlersTest.Authors.Commands
{
    public class UpdateAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorRepository> _mockAuthorRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateAuthorCommandHandler _handler;

        public UpdateAuthorCommandHandlerTests()
        {
            _mockAuthorRepository = new Mock<IAuthorRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateAuthorCommandHandler(_mockAuthorRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenAuthorIsUpdated()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author { Id = authorId, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1980, 1, 1) };

            var command = new UpdateAuthorCommand
            {
                AuthorId = authorId,
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1990, 2, 15)
            };

            var updatedAuthor = new Author
            {
                Id = command.AuthorId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                DateOfBirth = command.DateOfBirth
            };

            _mockAuthorRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetByIdSpecification<Author>>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(existingAuthor);

            _mockMapper.Setup(m => m.Map<Author>(command)).Returns(updatedAuthor);
            _mockAuthorRepository.Setup(r => r.UpdateAsync(updatedAuthor, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            _mockAuthorRepository.Verify(r => r.UpdateAsync(updatedAuthor, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorId = Guid.NewGuid();

            var command = new UpdateAuthorCommand
            {
                AuthorId = authorId,
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1990, 2, 15)
            };

            _mockAuthorRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetByIdSpecification<Author>>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync((Author)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(ErrorMessages.NotFoundMessage(ErrorMessageType.Author), result.Errors);
            _mockAuthorRepository.Verify(r => r.UpdateAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

}
