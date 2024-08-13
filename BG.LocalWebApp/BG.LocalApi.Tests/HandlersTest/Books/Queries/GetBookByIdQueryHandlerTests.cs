using Ardalis.Specification;
using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Book;
using BG.LocalApi.Application.Handlers.Books.Queries;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Enums;
using BG.LocalApi.Domain.Interfaces.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalApi.Tests.HandlersTest.Books.Queries
{
    public class GetBookByIdQueryHandlerTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetBookByIdQueryHandler _handler;

        public GetBookByIdQueryHandlerTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetBookByIdQueryHandler(_mockBookRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_BookExists_ShouldReturnBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new Book { Id = bookId, Title = "Test Book", PublicationYear = 2022, Genre = Genre.Fiction };
            var bookDto = new BookDto { Id = bookId, Title = "Test Book", PublicationYear = 2022, Genre = "Fiction" };

            _mockBookRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ISpecification<Book>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(book);
            _mockMapper.Setup(m => m.Map<BookDto>(book)).Returns(bookDto);

            var query = new GetBookByIdQuery(bookId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(bookDto, result.Data);
            _mockBookRepository.Verify(repo => repo.FirstOrDefaultAsync(It.IsAny<ISpecification<Book>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task Handle_BookDoesNotExist_ShouldReturnFailure()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            _mockBookRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ISpecification<Book>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync((Book)null);

            var query = new GetBookByIdQuery(bookId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Null(result.Data);
            Assert.Contains("not found", result.Errors.FirstOrDefault()?.ToLower());
            _mockBookRepository.Verify(repo => repo.FirstOrDefaultAsync(It.IsAny<ISpecification<Book>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}
