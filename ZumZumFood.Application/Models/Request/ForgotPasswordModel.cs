using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Application.Models.Request
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
    }
}
