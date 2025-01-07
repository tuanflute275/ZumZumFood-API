namespace ZumZumFood.Domain.Entities
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

        [Column("ComboId")]
        public int? ComboId { get; set; }
        [ForeignKey("ComboId")]
        [JsonIgnore]
        public virtual Combo Combo { get; set; }

        public string? OrderDetailType { get; set; } // Combo, Product
    }
}
