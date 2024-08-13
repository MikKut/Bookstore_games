using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalWebApp.Application.Common.Specifications.Base;
using BG.LocalWebApp.Common.Constants;
using BG.LocalWebApp.Common.Models;
using MediatR;

namespace BG.LocalApi.Application.Handlers.Authors.Commands
{
    public class DeleteAuthorCommand : IRequest<Result>
    {
        public Guid Id { get; set; }

        public DeleteAuthorCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Result>
    {
        private readonly IAuthorRepository _authorRepository;

        public DeleteAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<Result> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var findExistingSpec = new GetByIdSpecification<Author>(request.Id);
            var existingEntity = await _authorRepository.FirstOrDefaultAsync(findExistingSpec, cancellationToken);
            if (existingEntity == null)
            {
                return Result.Failure(ErrorMessages.NotFoundMessage(ErrorMessageType.Author));
            }

            await _authorRepository.DeleteAsync(existingEntity, cancellationToken);
            return Result.Success();
        }
    }
}
