using System.ComponentModel.DataAnnotations.Schema;

namespace ZumZumFood.Application.Models.DTOs
{
    public class ProductDetailDTO
    {
        public int ProductDetailId { get; set; }
        public string Name { get; set; } // Tên sản phẩm, topping, combo
        public string Type { get; set; } // Loại sản phẩm:  "Topping"
        public string? Size { get; set; } // Kích thước món (nếu có), VD: "Nhỏ", "Vừa", "Lớn"
        public decimal? Price { get; set; } // Giá của sản phẩm hoặc topping
        public int? Quantity { get; set; } // Số lượng còn trong kho
        public string? Description { get; set; } // Mô tả sản phẩm hoặc chi tiết thêm
        public bool IsAvailable { get; set; } = true;
    }
}
