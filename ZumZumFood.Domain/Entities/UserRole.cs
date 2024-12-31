using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Domain.Entities
{
    [Table("UserRoles")]
    public class UserRole : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleId { get; set; }

        [Column]
        [Required]
        [StringLength(255)]
        public int UserId { get; set; }

        [Column]
        [Required]
        [StringLength(255)]
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
