using AutoMapper;
using BG.LocalApi.Application.Common.DTOs.Book;
using BG.LocalApi.Application.Handlers.Books.Commands;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Enums;
using BG.LocalWebApp.Common.Helpers;

namespace BG.LocalApi.Application.Common.Mappings
{
    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            // Mapping from CreateBookCommand to Book
            CreateMap<CreateBookCommand, Book>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => EnumHelper.GetEnumValue<Genre>(src.Genre)));

            // Mapping from UpdateBookCommand to Book
            CreateMap<UpdateBookCommand, Book>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => EnumHelper.GetEnumValue<Genre>(src.Genre)));

            // Mapping from Book to BookDto
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.ToString()));

            // Optional: Mapping from BookDto to Book
            CreateMap<BookDto, Book>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => EnumHelper.GetEnumValue<Genre>(src.Genre)));
        }
    }
}
