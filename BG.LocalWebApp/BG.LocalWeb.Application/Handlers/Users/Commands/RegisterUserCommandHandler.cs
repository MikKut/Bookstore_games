using AutoMapper;
using BG.LocalWeb.Application.Common.DTOs.User;
using BG.LocalWeb.Domain.Entities;
using BG.LocalWeb.Domain.Interfaces.Repositories;
using BG.LocalWeb.Domain.Interfaces.Services;
using BG.LocalWeb.Domain.Specifications.UserSpecifications;
using BG.LocalWebApp.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BG.LocalWeb.Application.Handlers.Users.Commands
{
    public class RegisterUserCommand : IRequest<Result<UserResponseDto>>
    {
        public UserCreateDto Request { get; }

        public RegisterUserCommand(UserCreateDto request)
        {
            Request = request;
        }
    }
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IJwtTokenService jwtTokenService, IMapper mapper)
        {
            _jwtTokenService = jwtTokenService;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<Result<UserResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUserSpec = new UserByUsernameSpecification(request.Request.Username);
            var existingUser = await _userRepository.FirstOrDefaultAsync(existingUserSpec, cancellationToken);

            if (existingUser != null)
            {
                return Result<UserResponseDto>.Failure("Username is already taken.");
            }
            var user = new User()
            {
                UserName = request.Request.Username,
                FirstName = request.Request.FirstName,
                LastName = request.Request.LastName,
                DateOfBirth = request.Request.DateOfBirth,
                Address = request.Request.Address
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Request.Password);
            var userDto = _mapper.Map<UserDto>(user);
            var renovatedUser = await _userRepository.AddAsync(user, cancellationToken);
            var token = _jwtTokenService.GenerateToken(renovatedUser);
            userDto.Id = renovatedUser.Id;
            var res = new UserResponseDto { Token = token, User = userDto };

            return Result<UserResponseDto>.Success(res);
        }
    }
}
