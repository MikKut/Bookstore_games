using AutoMapper;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalWebApp.Application.Common.Specifications.Base;
using BG.LocalWebApp.Common.Constants;
using BG.LocalWebApp.Common.Models;
using MediatR;

namespace BG.LocalApi.Application.Handlers.Authors.Commands
{
    public class UpdateAuthorCommand : IRequest<Result>
    {
        public Guid AuthorId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = default!;
    }

    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Result>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public UpdateAuthorCommandHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var findExistingSpec = new GetByIdSpecification<Author>(request.AuthorId);
            var existingEntity = await _authorRepository.FirstOrDefaultAsync(findExistingSpec, cancellationToken);
            if (existingEntity == null)
            {
                return Result.Failure(ErrorMessages.NotFoundMessage(ErrorMessageType.Author));
            }

            var author = _mapper.Map<Author>(request);
            await _authorRepository.UpdateAsync(author, cancellationToken);
            return Result.Success();
        }
    }
}