using System.ComponentModel.DataAnnotations;

namespace URLShortenerAPI.Models.User
{
    public class LoginModel
    {
        [Required]
        public string Email { get; init; }

        [Required]
        public string Password { get; init; }
    }
}
