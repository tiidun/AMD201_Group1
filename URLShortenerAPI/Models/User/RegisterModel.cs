using System.ComponentModel.DataAnnotations;

namespace URLShortenerAPI.Models.User
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; init; }
    }
}
