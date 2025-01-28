namespace ZumZumFood.Application.Models.Request
{
    public class ProductCommentUpdateModel
    {
       
        // Message: Phải có nội dung và độ dài tối đa
        [Required(ErrorMessage = "Message is required.")]
        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
        public string Message { get; set; }
    }
}
