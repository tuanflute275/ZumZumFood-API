namespace ZumZumFood.Application.Models.DTOs
{
    public class BrandDTO : BaseDTO
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public string? OpenTime { get; set; }  // Thời gian mở cửa
        public string? CloseTime { get; set; }  // Thời gian đóng cửa
    }
}
