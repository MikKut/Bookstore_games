using AutoMapper;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalWebApp.Application.Common.Specifications.Base;
using BG.LocalWebApp.Common.Constants;
using BG.LocalWebApp.Common.Models;
using MediatR;

namespace BG.LocalApi.Application.Handlers.Books.Commands
{
    public class UpdateBookCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public string Genre { get; set; }
    }

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Result>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public UpdateBookCommandHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var findExistingSpec = new GetByIdSpecification<Book>(request.Id);
            var existingEntity = await _bookRepository.FirstOrDefaultAsync(findExistingSpec, cancellationToken);
            if (existingEntity == null)
            {
                return Result.Failure(ErrorMessages.NotFoundMessage(ErrorMessageType.Book));
            }

            var book = _mapper.Map<Book>(request);
            await _bookRepository.UpdateAsync(book, cancellationToken);
            return Result.Success();
        }
    }
}
