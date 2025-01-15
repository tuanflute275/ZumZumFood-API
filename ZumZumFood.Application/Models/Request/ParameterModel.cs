namespace ZumZumFood.Application.Models.Request
{
    public class ParameterModel
    {
        [Required(ErrorMessage = "ParaScope is required.")]
        [StringLength(100, ErrorMessage = "ParaScope cannot be longer than 100 characters.")]
        public string ParaScope { get; set; }

        [Required(ErrorMessage = "ParaName is required.")]
        public string ParaName { get; set; }

        [Required(ErrorMessage = "ParaType is required.")]
        [StringLength(10, ErrorMessage = "ParaType cannot be longer than 10 characters.")]
        public string ParaType { get; set; }
        public string? ParaShortValue { get; set; }
        public string? ParaLobValue { get; set; }
        public string? ParaDesc { get; set; }
        public bool? UserAccessibleFlag { get; set; } // Người dùng có thể truy cập (true/false)
        public bool? AdminAccessibleFlag { get; set; }// Admin có thể truy cập (true/false)
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
    }
}
