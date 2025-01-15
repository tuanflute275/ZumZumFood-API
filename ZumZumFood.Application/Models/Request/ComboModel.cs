namespace ZumZumFood.Application.Models.Request
{
    public class ComboModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? OldImage { get; set; }
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double Price { get; set; }
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "At least one product must be selected.")]
        [MinLength(1, ErrorMessage = "At least one product must be selected.")]
        public List<int> listProduct { get; set; } = new List<int>();
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
    }
}
