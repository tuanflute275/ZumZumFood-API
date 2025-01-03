namespace ZumZumFood.Application.Models.Request
{
    public class ProductCommentRequestModel
    {
        // Email: Phải là một địa chỉ email hợp lệ
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        // Name: Không được để trống và có độ dài tối đa
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        // Message: Phải có nội dung và độ dài tối đa
        [Required(ErrorMessage = "Message is required.")]
        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
        public string Message { get; set; }

        // UserId: Phải là một giá trị hợp lệ (ví dụ: không âm)
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public int UserId { get; set; }

        // ProductId: Phải là một giá trị hợp lệ (ví dụ: không âm)
        [Required(ErrorMessage = "ProductId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0.")]
        public int ProductId { get; set; }
    }
}
