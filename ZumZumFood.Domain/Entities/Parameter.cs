using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Domain.Entities
{
    [Table("Parameters")]
    public class Parameter : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParameterId { get; set; }

        [Required]
        [StringLength(50)]
        public string ParaScope { get; set; } // Phạm vi áp dụng (Global, Restaurant, Delivery, etc.)

        [Required]
        [StringLength(100)]
        public string ParaName { get; set; } // Tên của tham số (định danh)

        [StringLength(255)]
        public string? ParaShortValue { get; set; } // Giá trị ngắn

        public string? ParaLobValue { get; set; } // Giá trị chi tiết hoặc phức tạp (JSON, chuỗi dài)

        public string? ParaDesc { get; set; } // Mô tả tham số

        [Required]
        [StringLength(50)]
        public string ParaType { get; set; } // Loại tham số (String, Number, JSON, Boolean, etc.)
        
        public bool UserAccessibleFlag { get; set; } // Người dùng có thể truy cập (true/false)
        public bool AdminAccessibleFlag { get; set; } // Admin có thể truy cập (true/false)
    }
}
