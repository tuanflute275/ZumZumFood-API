namespace ZumZumFood.Application.Models.Request
{
    public class OrderUpdateModel
    {
        [Required(ErrorMessage = "Status is required.")]
        public int Status { get; set; }
    }
}
