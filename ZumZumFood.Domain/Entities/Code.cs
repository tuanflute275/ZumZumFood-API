namespace ZumZumFood.Domain.Entities
{
    [Table("Codes")]
    public class Code : BaseEntity
    {
        [Key]
        [Column(TypeName = "nvarchar(100)")]
        public string CodeId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? CodeDes { get; set; }
    }
}
