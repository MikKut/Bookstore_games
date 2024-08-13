using Ardalis.Specification;
using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Author;
using BG.LocalApi.Application.Handlers.Authors.Queries;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalApi.Tests.HandlersTest.Authors.Queries
{
    public class GetAuthorByIdQueryHandlerTests
    {
        private readonly Mock<IAuthorRepository> _mockAuthorRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAuthorByIdQueryHandler _handler;

        public GetAuthorByIdQueryHandlerTests()
        {
            _mockAuthorRepository = new Mock<IAuthorRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetAuthorByIdQueryHandler(_mockAuthorRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAuthorDto_WhenAuthorExists()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = new Author { Id = authorId, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1980, 1, 1) };
            var authorDto = new AuthorDto { Id = authorId, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1980, 1, 1) };

            _mockAuthorRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<ISpecification<Author>>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(author);

            _mockMapper.Setup(m => m.Map<AuthorDto>(author)).Returns(authorDto);

            var query = new GetAuthorByIdQuery(authorId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Equal(authorDto, result.Data);
            _mockAuthorRepository.Verify(r => r.FirstOrDefaultAsync(It.IsAny<ISpecification<Author>>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.Map<AuthorDto>(author), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorId = Guid.NewGuid();

            _mockAuthorRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<ISpecification<Author>>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync((Author?)null);

            var query = new GetAuthorByIdQuery(authorId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Null(result.Data);

            // Use Assert.Equal to match the exact error message
            var expectedErrorMessage = "Author not found.";
            Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault());

            _mockAuthorRepository.Verify(r => r.FirstOrDefaultAsync(It.IsAny<ISpecification<Author>>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.Map<AuthorDto>(It.IsAny<Author>()), Times.Never);
        }
    }
}
