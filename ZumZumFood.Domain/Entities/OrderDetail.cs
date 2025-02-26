﻿namespace ZumZumFood.Domain.Entities
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OrderDetailId")]
        public int OrderDetailId { get; set; }

        [Column("Quantity")]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Column("TotalMoney")]
        [Range(0, double.MaxValue)]
        public double TotalMoney { get; set; }

        [Column("OrderId")]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [JsonIgnore]
        public virtual Order Order { get; set; }

        [Column("ProductId")]
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        [JsonIgnore]
        public virtual Product Product { get; set; }

        [Column("ComboProductId")]
        public int? ComboProductId { get; set; }
        [ForeignKey("ComboProductId")]
        [JsonIgnore]
        public virtual ComboProduct ComboProduct { get; set; }

        public string? OrderDetailType { get; set; } // Combo, Product
    }
}
