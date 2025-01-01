namespace ZumZumFood.Application.Models.DTOs
{
    public class UserProductDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string? UserAvatar { get; set; }
        public string UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        public string? UserAddress { get; set; }
        public int? UserGender { get; set; } = 0; // 0 Chưa xác định, 1 Nam , 2 Nữ
        public DateTime? DateOfBirth { get; set; }           // Ngày sinh
        public string? PlaceOfBirth { get; set; }            // Nơi sinh
        public string? Nationality { get; set; } 
    }
}
