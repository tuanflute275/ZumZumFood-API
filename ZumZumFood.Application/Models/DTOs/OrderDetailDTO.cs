namespace ZumZumFood.Application.Models.DTOs
{
    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public double TotalMoney { get; set; }
        public string? OrderDetailType { get; set; }
        public ProductDTO Product { get; set; }
        public ComboDTO Combo { get; set; }
    }
}
