using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Book;
using BG.LocalApi.Application.Common.Requests;
using BG.LocalApi.Domain.Enums;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalApi.Domain.Specifications.BookSpecifications;
using BG.LocalWebApp.Common.Helpers;
using BG.LocalWebApp.Common.Models;
using MediatR;
using System.Collections.Immutable;

namespace BG.LocalApi.Application.Handlers.Books.Queries
{
    public class GetAllBooksQuery : IRequest<PagedResult<BookDto>>
    {
        public BooksPagedFilterRequest Filter { get; set; }

        public GetAllBooksQuery(BooksPagedFilterRequest filter)
        {
            Filter = filter;
        }
    }

    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, PagedResult<BookDto>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetAllBooksQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            Genre? genre = request.Filter.Genre == null ? null : EnumHelper.GetEnumValue<Genre>(request.Filter.Genre);
            var spec = new BooksPagedFilterSpecification(genre, request.Filter.Title, request.Filter.PageNumber, request.Filter.PageSize);
            var books = await _bookRepository.ListAsync(spec, cancellationToken);
            var totalCount = await _bookRepository.CountAsync(spec, cancellationToken);

            var bookDtos = books.Select(x => _mapper.Map<BookDto>(x)).ToList();

            return new PagedResult<BookDto>(bookDtos, request.Filter.PageNumber, request.Filter.PageSize, totalCount);
        }
    }
}
