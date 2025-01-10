namespace ZumZumFood.Application.Models.Request
{
    public class CouponModel
    {
        [Required(ErrorMessage = "Code is required.")]
        [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters.")]
        public string Code { get; set; }

        [Range(0, 100, ErrorMessage = "Percent must be between 0 and 100.")]
        public int Percent { get; set; } = 0;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public string? Scope { get; set; }
        public int? ScopeId { get; set; }
    }
}
