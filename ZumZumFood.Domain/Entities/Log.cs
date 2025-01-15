namespace ZumZumFood.Domain.Entities
{
    [Table("Logs")]
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("logId")]
        public int LogId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? UserName { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? WorkTation { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? Request { get; set; }

        [Column(TypeName = "ntext")]
        public string? Response { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? IpAdress { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? KeyApi { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? Url { get; set; }

        [Column]
        public DateTime? TimeLogin { get; set; }

        [Column]
        public DateTime? TimeLogout { get; set; }

        [Column]
        public DateTime? TimeActionRequest { get; set; }
    }
}
