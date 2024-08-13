using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Author;
using BG.LocalApi.Application.Common.Requests;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalApi.Domain.Specifications.AuthorSpecificaitons;
using BG.LocalWebApp.Common.Models;
using MediatR;
using System.Collections.Immutable;

namespace BG.LocalApi.Application.Handlers.Authors.Queries
{
    public class GetAllAuthorsQuery : IRequest<PagedResult<AuthorDto>>
    {
        public AuthorsPagedFilterRequest Filter { get; set; }

        public GetAllAuthorsQuery(AuthorsPagedFilterRequest filter)
        {
            Filter = filter;
        }
    }
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, PagedResult<AuthorDto>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public GetAllAuthorsQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            var spec = new AuthorsPagedFilterSpecification(request.Filter.FirstName, request.Filter.LastName, request.Filter.PageSize, request.Filter.PageNumber);
            var authors = await _authorRepository.ListAsync(spec, cancellationToken);

            var totalCount = await _authorRepository.CountAsync(spec, cancellationToken);
            var authorDtos = authors.Select(x => _mapper.Map<AuthorDto>(x)).ToList();
            return new PagedResult<AuthorDto>(
                authorDtos,
                request.Filter.PageNumber,
                request.Filter.PageSize,
                totalCount
            );
        }
    }
}
