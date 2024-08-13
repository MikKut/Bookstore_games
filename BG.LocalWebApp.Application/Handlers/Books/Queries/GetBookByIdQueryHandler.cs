using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Book;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalWebApp.Application.Common.Specifications.Base;
using BG.LocalWebApp.Common.Constants;
using BG.LocalWebApp.Common.Models;
using MediatR;

namespace BG.LocalApi.Application.Handlers.Books.Queries
{
    public class GetBookByIdQuery : IRequest<Result<BookDto>>
    {
        public Guid Id { get; set; }

        public GetBookByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Result<BookDto>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public GetBookByIdQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<Result<BookDto>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var findExistingSpec = new GetByIdSpecification<Book>(request.Id);
            var existingEntity = await _bookRepository.FirstOrDefaultAsync(findExistingSpec, cancellationToken);
            if (existingEntity == null)
            {
                return Result<BookDto>.Failure(ErrorMessages.NotFoundMessage(ErrorMessageType.Book));
            }

            var res = _mapper.Map<BookDto>(existingEntity);
            return Result<BookDto>.Success(res);
        }
    }
}
