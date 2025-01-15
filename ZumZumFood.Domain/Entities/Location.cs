namespace ZumZumFood.Domain.Entities
{
    [Table("Locations")]
    public class Location : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column]
        public int Id { get; set; }

        [Column]
        public int? Status { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string? Code { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? NameEN { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? NameZH { get; set; }
    }
}
