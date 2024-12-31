using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Domain.Entities
{
    [Table("ProductImages")]
    public class ProductImage : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ProductImageId")]
        public int ProductImageId { get; set; }

        [Column("Path", TypeName = "nvarchar(200)")]
        public string Path { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
