using AutoMapper;
using BG.LocalWeb.Application.Common.DTOs.User;
using BG.LocalWeb.Domain.Entities;
using BG.LocalWebApp.Common.ValueObjects;

namespace BG.LocalWeb.Application.Common.Mappings
{
    internal class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Mapping from User to UserDto
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.Value));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => new DateOfBirth(src.DateOfBirth)));

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));

            CreateMap<UserDto, UserResponseDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));
        }
    }
}