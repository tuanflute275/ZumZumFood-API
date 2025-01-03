namespace ZumZumFood.Application.Models.Request
{
    public class CategoryModel
    {
        public IFormFile? ImageFile { get; set; }
        public string? OldImage { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name can't be longer than 100 characters.")]
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? Description { get; set; } = "";
    }
}
