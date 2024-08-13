using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Book;
using BG.LocalApi.Application.Common.Requests;
using BG.LocalApi.Application.Handlers.Books.Queries;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Enums;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalApi.Domain.Specifications.BookSpecifications;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalApi.Tests.HandlersTest.Books.Queries
{
    public class GetAllBooksQueryHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllBooksQueryHandler _handler;

        public GetAllBooksQueryHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllBooksQueryHandler(_bookRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsPagedResultOfBookDto()
        {
            // Arrange
            var filter = new BooksPagedFilterRequest
            {
                Genre = "Fiction",
                Title = "Test",
                PageNumber = 1,
                PageSize = 10
            };

            var id = new Guid();
            var query = new GetAllBooksQuery(filter);
            var books = new List<Book> { new Book { Id = id, Title = "Test Book" } };
            var bookDto = new BookDto { Id = id, Title = "Test Book" };

            _bookRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<BooksPagedFilterSpecification>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(books);

            _bookRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<BooksPagedFilterSpecification>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(1);

            _mapperMock.Setup(mapper => mapper.Map<BookDto>(It.IsAny<Book>()))
                       .Returns(bookDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.TotalCount);
            Assert.Single(result.Items);
            Assert.Equal(bookDto.Title, result.Items.First().Title);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyPagedResult_WhenNoBooksFound()
        {
            // Arrange
            var filter = new BooksPagedFilterRequest
            {
                Genre = "Fiction",
                Title = "NonExistingTitle",
                PageNumber = 1,
                PageSize = 10
            };

            var query = new GetAllBooksQuery(filter);
            var emptyBookList = new List<Book>();

            _bookRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<BooksPagedFilterSpecification>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(emptyBookList);

            _bookRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<BooksPagedFilterSpecification>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
        }

        [Fact]
        public async Task Handle_MapsBooksToBookDtosCorrectly()
        {
            // Arrange
            var filter = new BooksPagedFilterRequest
            {
                Genre = null,
                Title = "Test",
                PageNumber = 1,
                PageSize = 10
            };

            var id1 = new Guid("5c383d16-870f-45dd-a33c-a45380d430ff");
            var id2 = new Guid("f7376f30-fac9-40cd-982b-784498699a42");
            var query = new GetAllBooksQuery(filter);
            var books = new List<Book>
        {
            new Book { Id = id1, Title = "Book 1" },
            new Book { Id = id2, Title = "Book 2" }
        };

            _bookRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<BooksPagedFilterSpecification>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(books);

            _bookRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<BooksPagedFilterSpecification>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(2);

            _mapperMock.Setup(mapper => mapper.Map<BookDto>(It.Is<Book>(b => b.Id == id1)))
                       .Returns(new BookDto { Id = id1, Title = "Book 1" });

            _mapperMock.Setup(mapper => mapper.Map<BookDto>(It.Is<Book>(b => b.Id == id2)))
                       .Returns(new BookDto { Id = id2, Title = "Book 2" });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Contains(result.Items, dto => dto.Title == "Book 1");
            Assert.Contains(result.Items, dto => dto.Title == "Book 2");
        }
    }
}
