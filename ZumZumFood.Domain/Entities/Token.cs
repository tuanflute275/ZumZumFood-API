using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Domain.Entities
{
    [Table("Tokens")]
    public class Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "ntext")]
        public string AccessToken { get; set; }

        // Loại token (ví dụ: AccessToken, RefreshToken), tùy chọn.
        public string? TokenType { get; set; }

        // true là token mobile
        public bool IsMobile { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; } // Thời gian hết hạn của token

        [Required] // Trạng thái thu hồi token(true nếu đã thu hồi, false nếu còn hiệu lực).
        public bool IsRevoked { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; } = DateTime.Now; // Thời điểm tạo token

        //Chuỗi token dùng để làm mới AccessToken.
        public string RefreshToken { get; set; }

        // // Thời gian hết hạn của refresh token
        public DateTime RefreshTokenDate { get; set; }

        // Liên kết với User
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
