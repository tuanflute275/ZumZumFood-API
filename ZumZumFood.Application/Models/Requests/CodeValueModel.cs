﻿namespace ZumZumFood.Application.Models.Request
{
    public class CodeValueModel
    {
        [Required(ErrorMessage = "CodeId is required.")]
        public string CodeId { get; set; } = null!;
        public string? ParentCodeValue { get; set; }

        [Required(ErrorMessage = "CodeValue is required.")]
        public string CodeValue { get; set; } = null!;
        public string? CodeValueDes { get; set; }
        public string? CodeValueDes1 { get; set; }
        public string? CodeValueDes2 { get; set; }
        public string? CodeValueDes3 { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
    }
}
