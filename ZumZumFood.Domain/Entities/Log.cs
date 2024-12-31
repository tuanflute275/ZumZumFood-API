using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Domain.Entities
{
    [Table("Logs")]
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("logId")]
        public int LogId { get; set; }

        [Column("userName", TypeName = "nvarchar(255)")]
        public string? UserName { get; set; }

        [Column("workTation", TypeName = "nvarchar(255)")]
        public string? WorkTation { get; set; }

        [Column("request", TypeName = "nvarchar(255)")]
        public string? Request { get; set; }

        [Column("response", TypeName = "ntext")]
        public string? Response { get; set; }

        [Column("ipAdress", TypeName = "nvarchar(255)")]
        public string? IpAdress { get; set; }

        [Column("timeLogin")]
        public DateTime? TimeLogin { get; set; }

        [Column("timeLogout")]
        public DateTime? TimeLogout { get; set; }

        [Column("timeActionRequest")]
        public DateTime? TimeActionRequest { get; set; }
    }
}
