using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Application.Models.Request
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Username or email is required.")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(5, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }
    }
}
