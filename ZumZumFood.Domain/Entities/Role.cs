using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Domain.Entities
{
    [Table("Roles")]
    public class Role : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(200)")]
        public string RoleName { get; set; }

        [Column(TypeName = "ntext")]
        public string? RoleDescription { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
