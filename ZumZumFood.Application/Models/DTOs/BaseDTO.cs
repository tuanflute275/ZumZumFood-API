namespace ZumZumFood.Application.Models.DTOs
{
    public class BaseDTO
    {
        public string? CreateBy { get; set; } 
        public string? CreateDate { get; set; }
        public string? UpdateBy { get; set; } 
        public string? UpdateDate { get; set; } 
        public string? DeleteBy { get; set; }
        public string? DeleteDate { get; set; } 
        public bool? DeleteFlag { get; set; }
    }
}
