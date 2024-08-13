namespace BG.LocalWeb.Application.Common.DTOs.User
{
    public class UserResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; }
    }
}
