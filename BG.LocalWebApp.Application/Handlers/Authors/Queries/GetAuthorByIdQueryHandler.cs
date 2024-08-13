using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Author;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalWebApp.Application.Common.Specifications.Base;
using BG.LocalWebApp.Common.Constants;
using BG.LocalWebApp.Common.Models;
using MediatR;

namespace BG.LocalApi.Application.Handlers.Authors.Queries
{
    public class GetAuthorByIdQuery : IRequest<Result<AuthorDto>>
    {
        public Guid Id { get; set; }

        public GetAuthorByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Result<AuthorDto>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<Result<AuthorDto>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var findExistingSpec = new GetByIdSpecification<Author>(request.Id);
            var existingEntity = await _authorRepository.FirstOrDefaultAsync(findExistingSpec, cancellationToken);
            if (existingEntity == null)
            {
                return Result<AuthorDto>.Failure(ErrorMessages.NotFoundMessage(ErrorMessageType.Author));
            }

            var res = _mapper.Map<AuthorDto>(existingEntity);
            return Result<AuthorDto>.Success(res);
        }
    }
}
