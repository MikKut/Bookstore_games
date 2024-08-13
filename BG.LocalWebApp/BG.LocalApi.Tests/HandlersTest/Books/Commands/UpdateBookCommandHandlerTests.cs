using AutoMapper;
using BG.LocalApi.Application.Handlers.Books.Commands;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Enums;
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
    public class UpdateBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateBookCommandHandler _handler;

        public UpdateBookCommandHandlerTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateBookCommandHandler(_mockBookRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_BookExists_ShouldUpdateBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new UpdateBookCommand
            {
                Id = bookId,
                Title = "Updated Title",
                PublicationYear = 2023,
                Genre = "Fiction"
            };

            var existingBook = new Book { Id = bookId, Title = "Original Title", PublicationYear = 2020, Genre = GetGenre("NonFiction") };
            var updatedBook = new Book { Id = bookId, Title = command.Title, PublicationYear = command.PublicationYear, Genre = GetGenre("NonFiction") };

            _mockBookRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<GetByIdSpecification<Book>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(existingBook);

            _mockMapper.Setup(m => m.Map<Book>(command)).Returns(updatedBook);

            _mockBookRepository.Setup(repo => repo.UpdateAsync(updatedBook, It.IsAny<CancellationToken>()))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            _mockBookRepository.Verify(repo => repo.UpdateAsync(updatedBook, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_BookDoesNotExist_ShouldReturnFailure()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new UpdateBookCommand
            {
                Id = bookId,
                Title = "Updated Title",
                PublicationYear = 2023,
                Genre = "Fiction"
            };

            _mockBookRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<GetByIdSpecification<Book>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.True(result.HasErrors);
            Assert.Contains("not found", result.Errors[0].ToLower());
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private Genre GetGenre(string stringValue)
        {
            var enumValue = (Genre)Enum.Parse(typeof(Genre), stringValue);
            return enumValue;
        }
    }

}
