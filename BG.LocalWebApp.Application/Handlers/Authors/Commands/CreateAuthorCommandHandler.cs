using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Author;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalApi.Domain.Specifications.AuthorSpecificaitons;
using BG.LocalWebApp.Common.Models;
using MediatR;

namespace BG.LocalApi.Application.Handlers.Authors.Commands
{
    public class CreateAuthorCommand : IRequest<Result<AuthorDto>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Result<AuthorDto>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public CreateAuthorCommandHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<Result<AuthorDto>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = _mapper.Map<Author>(request);
            var findBookSpec = new AuthorByFirstLastNameAndBirthDateSpecification(author);
            var existingEntity = await _authorRepository.FirstOrDefaultAsync(findBookSpec, cancellationToken);
            if (existingEntity != null)
            {
                return Result<AuthorDto>.Failure($"Cannot create {author.LastName} {author.FirstName}: the author with such name and date birth already exists");
            }

            var createdAuthor = await _authorRepository.AddAsync(author, cancellationToken);
            var authorDto = _mapper.Map<AuthorDto>(createdAuthor);
            return Result<AuthorDto>.Success(authorDto);
        }
    }
}