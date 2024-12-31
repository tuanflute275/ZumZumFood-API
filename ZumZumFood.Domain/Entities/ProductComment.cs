using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Domain.Entities
{
    [Table("ProductComments")]
    public class ProductComment : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ProductCommentId")]
        public int ProductCommentId { get; set; }

        [EmailAddress]
        [Column("Email", TypeName = "nvarchar(200)")]
        public string Email { get; set; }
        [Column("Name", TypeName = "nvarchar(200)")]
        public string Name { get; set; }
        [Column("Message", TypeName = "ntext")]
        public string Message { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
