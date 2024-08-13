using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Author;
using BG.LocalApi.Application.Common.Requests;
using BG.LocalApi.Application.Handlers.Authors.Queries;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalApi.Domain.Specifications.AuthorSpecificaitons;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalApi.Tests.HandlersTest.Authors.Queries
{
    public class GetAllAuthorsQueryHandlerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllAuthorsQueryHandler _handler;

        public GetAllAuthorsQueryHandlerTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllAuthorsQueryHandler(_authorRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsPagedResultOfAuthorDto()
        {
            // Arrange
            var filter = new AuthorsPagedFilterRequest
            {
                FirstName = "John",
                LastName = "Doe",
                PageNumber = 1,
                PageSize = 10
            };

            var id = new Guid();
            var query = new GetAllAuthorsQuery(filter);
            var authors = new List<Author> { new Author { Id = id, FirstName = "John", LastName = "Doe" } };
            var authorDto = new AuthorDto { Id = id, FirstName = "John", LastName = "Doe" };

            _authorRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<AuthorsPagedFilterSpecification>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(authors);

            _authorRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<AuthorsPagedFilterSpecification>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(1);

            _mapperMock.Setup(mapper => mapper.Map<AuthorDto>(It.IsAny<Author>()))
                       .Returns(authorDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.TotalCount);
            Assert.Single(result.Items);
            Assert.Equal(authorDto.FirstName, result.Items.First().FirstName);
            Assert.Equal(authorDto.LastName, result.Items.First().LastName);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyPagedResult_WhenNoAuthorsFound()
        {
            // Arrange
            var filter = new AuthorsPagedFilterRequest
            {
                FirstName = "NonExistingFirstName",
                LastName = "NonExistingLastName",
                PageNumber = 1,
                PageSize = 10
            };

            var query = new GetAllAuthorsQuery(filter);
            var emptyAuthorList = new List<Author>();

            _authorRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<AuthorsPagedFilterSpecification>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(emptyAuthorList);

            _authorRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<AuthorsPagedFilterSpecification>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
        }

        [Fact]
        public async Task Handle_MapsAuthorsToAuthorDtosCorrectly()
        {
            // Arrange
            var filter = new AuthorsPagedFilterRequest
            {
                FirstName = "Jane",
                LastName = "Smith",
                PageNumber = 1,
                PageSize = 10
            };
            var id1 = new Guid("5c383d16-870f-45dd-a33c-a45380d430ff");
            var id2 = new Guid("f7376f30-fac9-40cd-982b-784498699a42");
            var query = new GetAllAuthorsQuery(filter);
            var authors = new List<Author>
        {
            new Author { Id = id1, FirstName = "Jane", LastName = "Smith" },
            new Author { Id = id2, FirstName = "Emily", LastName = "Doe" }
        };

            _authorRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<AuthorsPagedFilterSpecification>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(authors);

            _authorRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<AuthorsPagedFilterSpecification>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(2);

            _mapperMock.Setup(mapper => mapper.Map<AuthorDto>(It.Is<Author>(a => a.Id == id1)))
                       .Returns(new AuthorDto { Id = id1, FirstName = "Jane", LastName = "Smith" });

            _mapperMock.Setup(mapper => mapper.Map<AuthorDto>(It.Is<Author>(a => a.Id == id2)))
                       .Returns(new AuthorDto { Id = id2, FirstName = "Emily", LastName = "Doe" });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Contains(result.Items, dto => dto.FirstName == "Jane" && dto.LastName == "Smith");
            Assert.Contains(result.Items, dto => dto.FirstName == "Emily" && dto.LastName == "Doe");
        }
    }
}
