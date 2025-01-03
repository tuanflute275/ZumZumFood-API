namespace ZumZumFood.Application.Models.Request
{
    public class RestaurantModel
    {
        [Required(ErrorMessage = "Restaurant name is required.")]
        [StringLength(100, ErrorMessage = "Restaurant name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "Address can't be longer than 255 characters.")]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public string? OpenTime { get; set; } = "00:00";
        public string? CloseTime { get; set; } = "23:00";
    }
}
