using BG.LocalApi.Application.Handlers.Books.Commands;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalWebApp.Application.Common.Specifications.Base;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalApi.Tests.HandlersTest.Books.Commands
{
    public class DeleteBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly DeleteBookCommandHandler _handler;

        public DeleteBookCommandHandlerTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _handler = new DeleteBookCommandHandler(_mockBookRepository.Object);
        }

        [Fact]
        public async Task Handle_BookExists_ShouldDeleteBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new Book { Id = bookId };

            _mockBookRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<GetByIdSpecification<Book>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(book);

            _mockBookRepository.Setup(repo => repo.DeleteAsync(book, It.IsAny<CancellationToken>()))
                               .Returns(Task.CompletedTask);

            var command = new DeleteBookCommand(bookId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            _mockBookRepository.Verify(repo => repo.DeleteAsync(book, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_BookDoesNotExist_ShouldReturnFailure()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            _mockBookRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<GetByIdSpecification<Book>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync((Book)null);

            var command = new DeleteBookCommand(bookId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.True(result.HasErrors);
            Assert.Contains("not found", result.Errors[0].ToLower());
            _mockBookRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
