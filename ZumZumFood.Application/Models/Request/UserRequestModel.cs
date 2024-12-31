using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Application.Models.RequestModel
{
    public class UserRequestModel
    {
        [Required(ErrorMessage = "UserName is required.")]
        [StringLength(100, ErrorMessage = "UserName cannot exceed 100 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "FullName is required.")]
        [StringLength(200, ErrorMessage = "FullName cannot exceed 200 characters.")]
        public string FullName { get; set; }
        public IFormFile? ImageFile { get; set; }

        public string? OldImage { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string? PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }

        [Range(0, 2, ErrorMessage = "Gender must be 0 (undefined), 1 (Male), or 2 (Female).")]
        public int? Gender { get; set; } = 0;

        [Range(0, 2, ErrorMessage = "Active status must be 0 (inactive), 1 (active), or 2 (banned).")]
        public int Active { get; set; } = 1;

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; } // Ngày sinh

        [StringLength(100, ErrorMessage = "Place of birth cannot exceed 100 characters.")]
        public string? PlaceOfBirth { get; set; } // Nơi sinh

        [StringLength(100, ErrorMessage = "Nationality cannot exceed 100 characters.")]
        public string? Nationality { get; set; } // Quốc tịch của người dùng.

        [StringLength(500, ErrorMessage = "User bio cannot exceed 500 characters.")]
        public string? UserBio { get; set; } // Mô tả ngắn về người dùng.

        public string? SocialLinks { get; set; } // Liên kết đến các tài khoản mạng xã hội
    }

}
