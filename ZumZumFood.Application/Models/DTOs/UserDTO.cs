namespace ZumZumFood.Application.Models.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string? UserAvatar { get; set; }
        public string UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        public string? UserAddress { get; set; }
        public int? UserGender { get; set; } = 0; // 0 Chưa xác định, 1 Nam , 2 Nữ
        public int UserActive { get; set; } = 1;   // 1 mở khóa, 0 tạm khóa, 2 cấm   
        public DateTime? LastLoginDate { get; set; }            // Lưu thời điểm đăng nhập gần nhất.
        public DateTime? DateOfBirth { get; set; }           // Ngày sinh
        public string? PlaceOfBirth { get; set; }            // Nơi sinh
        public string? Nationality { get; set; }             // Quốc tịch của người dùng.
        public string? UserBio { get; set; }                 // Mô tả ngắn về người dùng.
        public string? SocialLinks { get; set; }
        public string? Role { get; set; }
    }
}
