using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Author;
using BG.LocalApi.Application.Handlers.Authors.Commands;
using BG.LocalApi.Domain.Entities;
using BG.LocalWebApp.Common.ValueObjects;

namespace BG.LocalApi.Application.Common.Mappings
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            // Map from UpdateAuthorCommand to Author
            CreateMap<UpdateAuthorCommand, Author>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            // Map from CreateAuthorCommand to Author
            CreateMap<CreateAuthorCommand, Author>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => new DateOfBirth(src.DateOfBirth)));

            CreateMap<Author, UpdateAuthorCommand>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.Value));

            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.Value));
            // .ForMember(dest => dest.BookIds, opt => opt.MapFrom(src => src.Books.Select(x => x.Id)));

            CreateMap<AuthorDto, Author>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => new DateOfBirth(src.DateOfBirth)));
            // .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.BookIds.Select(x => new Book { Id = x })));
        }
    }
}
