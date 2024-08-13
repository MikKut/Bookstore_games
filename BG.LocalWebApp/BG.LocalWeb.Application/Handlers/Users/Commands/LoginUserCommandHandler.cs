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
    public class LoginUserCommand : IRequest<Result<UserResponseDto>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<UserResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IJwtTokenService jwtTokenService, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }

        public async Task<Result<UserResponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var spec = new UserByUsernameSpecification(request.Username);
            var user = await _userRepository.FirstOrDefaultAsync(spec, cancellationToken);

            if (user == null)
            {
                return Result<UserResponseDto>.Failure("Invalid username or password.");
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Result<UserResponseDto>.Failure("Invalid username or password.");
            }

            if (passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                if (!SetPasswordRehashToUserIfPossible(user, request.Password))
                {
                    return Result<UserResponseDto>.Failure("Could not rehash the password.");
                }

                await _userRepository.UpdateAsync(user, cancellationToken);
            }

            var token = _jwtTokenService.GenerateToken(user);

            var userDto = _mapper.Map<UserDto>(user);
            var response = new UserResponseDto { Token = token, User = userDto };

            return Result<UserResponseDto>.Success(response);
        }

        private bool SetPasswordRehashToUserIfPossible(User user, string newPassword)
        {
            int quantityOfTimesToRehash = 5;
            PasswordVerificationResult result;
            do
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
                result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, newPassword);

            } while (result == PasswordVerificationResult.SuccessRehashNeeded && quantityOfTimesToRehash-- > 0);

            if (result == PasswordVerificationResult.Failed)
            {
                return false;
            }

            return true;
        }
    }
}
