using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Book;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalApi.Domain.Specifications.BookSpecifications;
using BG.LocalWebApp.Common.Models;
using MediatR;

namespace BG.LocalApi.Application.Handlers.Books.Commands
{
    public class CreateBookCommand : IRequest<Result<BookDto>>
    {
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public string Genre { get; set; }
    }

    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Result<BookDto>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public CreateBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _authorRepository = authorRepository;
        }

        public async Task<Result<BookDto>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {

            var book = _mapper.Map<Book>(request);
            var findBookSpec = new BookByTitleGenreAndYearSpecification(book);
            var existingEntity = await _bookRepository.FirstOrDefaultAsync(findBookSpec, cancellationToken);
            if (existingEntity != null)
            {
                return Result<BookDto>.Failure($"Cannot create {book.Title}: the book with such title, genre and publication year already exists");
            }

            var createdBook = await _bookRepository.AddAsync(book, cancellationToken);
            var bookDto = _mapper.Map<BookDto>(createdBook);

            return Result<BookDto>.Success(bookDto);
        }
    }
}
