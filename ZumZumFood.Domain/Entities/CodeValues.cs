namespace ZumZumFood.Domain.Entities
{
    [Table("CodeValues")]
    public class CodeValues : BaseEntity
    {
        [Key]
        [Column(TypeName = "nvarchar(100)")]
        public string CodeId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string CodeValue { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? CodeValueDes { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? CodeValueDes1 { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? CodeValueDes2 { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? CodeValueDes3 { get; set; }
    }
}
