namespace ZumZumFood.Application.Models.DTOs
{
    public class UserMapperDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string? Avatar { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int? Gender { get; set; } = 0; // 0 Chưa xác định, 1 Nam , 2 Nữ
        public string? DateOfBirth { get; set; }           // Ngày sinh
        public string? PlaceOfBirth { get; set; }            // Nơi sinh
        public string? Nationality { get; set; } 
    }
}
