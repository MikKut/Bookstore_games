using System.ComponentModel.DataAnnotations.Schema;

namespace BG.LocalWeb.Application.Common.DTOs.User
{
    public class UserCreateDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}
