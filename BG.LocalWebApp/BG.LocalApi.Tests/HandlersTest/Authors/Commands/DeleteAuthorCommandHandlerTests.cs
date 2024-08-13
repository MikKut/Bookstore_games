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
    public class DeleteAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorRepository> _mockAuthorRepository;
        private readonly DeleteAuthorCommandHandler _handler;

        public DeleteAuthorCommandHandlerTests()
        {
            _mockAuthorRepository = new Mock<IAuthorRepository>();
            _handler = new DeleteAuthorCommandHandler(_mockAuthorRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenAuthorIsDeleted()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author { Id = authorId, FirstName = "John", LastName = "Doe" };

            _mockAuthorRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetByIdSpecification<Author>>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(existingAuthor);
            _mockAuthorRepository.Setup(r => r.DeleteAsync(existingAuthor, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var command = new DeleteAuthorCommand(authorId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            _mockAuthorRepository.Verify(r => r.DeleteAsync(existingAuthor, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorId = Guid.NewGuid();

            _mockAuthorRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetByIdSpecification<Author>>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync((Author)null!);

            var command = new DeleteAuthorCommand(authorId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(ErrorMessages.NotFoundMessage(ErrorMessageType.Author), result.Errors);
            _mockAuthorRepository.Verify(r => r.DeleteAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

}
