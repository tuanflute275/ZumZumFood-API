namespace ZumZumFood.Domain.Entities
{
    [Table("Categories")]
    public class Category : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column]
        public int CategoryId { get; set; }
        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string? Image { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Slug { get; set; }

        [Column]
        public bool? IsActive { get; set; } = true;  // Trạng thái kích hoạt

        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}
