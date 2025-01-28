using System.ComponentModel.DataAnnotations.Schema;

namespace ZumZumFood.Application.Models.Request
{
    public class ProductDetailModel
    {
        [Required(ErrorMessage = "ProductId is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; } // Tên sản phẩm, topping, combo
        public string? Type { get; set; } // Loại sản phẩm: "Topping"
        public string? Size { get; set; } // Kích thước món (nếu có), VD: "Nhỏ", "Vừa", "Lớn"

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal? Price { get; set; } // Giá của sản phẩm hoặc topping

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive integer.")]
        public int? Quantity { get; set; } // Số lượng còn trong kho
        public string? Description { get; set; } // Mô tả sản phẩm hoặc chi tiết thêm
        public bool IsAvailable { get; set; } = true; // Trạng thái còn hàng
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
    }
}
