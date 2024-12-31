using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Domain.Entities
{
    [Table("Users")]
    public class User : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(200)")]
        public string UserName { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string FullName { get; set; }

        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string? Avatar { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Password { get; set; }

        [Phone]
        [StringLength(15)]
        [Column(TypeName = "nvarchar(15)")]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string? Address { get; set; }

        [Column]
        public int? Gender { get; set; } = 0; // 0 Chưa xác định, 1 Nam , 2 Nữ

        [Column(TypeName = "int")]
        public int Active { get; set; } = 1;   // 1 mở khóa, 0 tạm khóa, 2 cấm    

        [Range(0, int.MaxValue)]
        [Column]
        public int? FailedLoginAttempts { get; set; } = 0;   // Số lần đăng nhập thất bại liên tiếp
                                                             // (dùng để khóa tài khoản tạm thời).
        [Column]
        public DateTime? UserCurrentTime { get; set; } // thời gian bắt đầu khóa tài khoản

        [Column]
        public DateTime? UserUnlockTime { get; set; } // thời gian tài khoản được mở khóa

        public DateTime? LastLoginDate { get; set; }            // Lưu thời điểm đăng nhập gần nhất.

        public DateTime? DateOfBirth { get; set; }           // Ngày sinh
        public string? PlaceOfBirth { get; set; }            // Nơi sinh
        public string? Nationality { get; set; }             // Quốc tịch của người dùng.
        public string? UserBio { get; set; }                 // Mô tả ngắn về người dùng.
        public string? SocialLinks { get; set; }             // Liên kết đến các tài khoản mạng xã hội (JSON)

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
