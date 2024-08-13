namespace BG.LocalApi.Application.Common.DTOs.Author
{
    public class AuthorDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = default!;
    }
}
