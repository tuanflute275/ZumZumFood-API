using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Application.Models.Request
{
    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
    }
}
