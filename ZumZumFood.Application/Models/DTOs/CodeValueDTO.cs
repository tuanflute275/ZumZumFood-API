namespace ZumZumFood.Application.Models.DTOs
{
    public class CodeValueDTO
    {
        public string CodeId { get; set; } = null!;

        public string? ParentCodeValue { get; set; }

        public string CodeValue { get; set; } = null!;

        public string? CodeValueDes { get; set; }

        public string? CodeValueDes1 { get; set; }

        public string? CodeValueDes2 { get; set; }

        public string? CodeValueDes3 { get; set; }

        public string? CreateDate { get; set; }
    }
}
