﻿namespace ZumZumFood.Domain.Entities
{
    [Table("Banners")]
    public class Banner : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BannerId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string Image { get; set; }

        [Column]
        public bool? IsActive { get; set; } = true;  // Trạng thái kích hoạt của banner
    }
}
