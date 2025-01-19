namespace ZumZumFood.Application.Models.Request
{
    public class CodeModel
    {
        [Required(ErrorMessage = "CodeId is required.")]
        public string CodeId { get; set; }
        public string? CodeDes { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
    }
}
