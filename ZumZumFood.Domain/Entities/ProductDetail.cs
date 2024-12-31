using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Domain.Entities
{
    [Table("ProductDetails")]
    public class ProductDetail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ProductDetailId")]
        public int ProductDetailId { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string Color { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string Size { get; set; }
        [Column]
        public int Quantity { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
