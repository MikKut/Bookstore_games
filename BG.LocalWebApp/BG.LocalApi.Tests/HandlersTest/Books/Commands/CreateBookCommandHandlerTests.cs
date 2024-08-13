using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Book;
using BG.LocalApi.Application.Handlers.Books.Commands;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Enums;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalApi.Domain.Specifications.BookSpecifications;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalApi.Tests.HandlersTest.Books.Commands
{
    public class CreateBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly Mock<IAuthorRepository> _mockAuthorRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateBookCommandHandler _handler;

        public CreateBookCommandHandlerTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _mockAuthorRepository = new Mock<IAuthorRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreateBookCommandHandler(_mockBookRepository.Object, _mockAuthorRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_BookDoesNotExist_ShouldCreateBook()
        {
            // Arrange
            var command = new CreateBookCommand
            {
                Title = "Test Book",
                PublicationYear = 2023,
                Genre = "Fiction"
            };

            var book = new Book { Title = command.Title, PublicationYear = command.PublicationYear, Genre = GetGenre(command.Genre) };
            var bookDto = new BookDto { Title = book.Title, PublicationYear = book.PublicationYear, Genre = book.Genre.ToString() };

            _mockMapper.Setup(m => m.Map<Book>(command)).Returns(book);
            _mockBookRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<BookByTitleGenreAndYearSpecification>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync((Book)null);

            _mockBookRepository.Setup(repo => repo.AddAsync(book, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(book);

            _mockMapper.Setup(m => m.Map<BookDto>(book)).Returns(bookDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Equal(bookDto.Title, result.Data.Title);
            _mockBookRepository.Verify(repo => repo.AddAsync(book, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_BookAlreadyExists_ShouldReturnFailure()
        {
            // Arrange
            var command = new CreateBookCommand
            {
                Title = "Existing Book",
                PublicationYear = 2023,
                Genre = "Fiction"
            };

            var book = new Book { Title = command.Title, PublicationYear = command.PublicationYear, Genre = GetGenre(command.Genre) };

            _mockMapper.Setup(m => m.Map<Book>(command)).Returns(book);
            _mockBookRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<BookByTitleGenreAndYearSpecification>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.True(result.HasErrors);
            Assert.Contains("Cannot create", result.Errors[0]);
            _mockBookRepository.Verify(repo => repo.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private Genre GetGenre(string stringValue)
        {
            var enumValue = (Genre)Enum.Parse(typeof(Genre), stringValue);
            return enumValue;
        }
    }
}

