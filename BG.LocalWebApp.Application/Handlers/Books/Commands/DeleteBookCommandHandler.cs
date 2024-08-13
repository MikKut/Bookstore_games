using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalWebApp.Application.Common.Specifications.Base;
using BG.LocalWebApp.Common.Constants;
using BG.LocalWebApp.Common.Models;
using MediatR;

namespace BG.LocalApi.Application.Handlers.Books.Commands
{
    public class DeleteBookCommand : IRequest<Result>
    {
        public Guid Id { get; set; }

        public DeleteBookCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Result>
    {
        private readonly IBookRepository _bookRepository;
        public DeleteBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var findExistingSpec = new GetByIdSpecification<Book>(request.Id);
            var existingEntity = await _bookRepository.FirstOrDefaultAsync(findExistingSpec, cancellationToken);
            if (existingEntity == null)
            {
                return Result.Failure(ErrorMessages.NotFoundMessage(ErrorMessageType.Book));
            }

            await _bookRepository.DeleteAsync(existingEntity, cancellationToken);

            return Result.Success();
        }
    }
}
