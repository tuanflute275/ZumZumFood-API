namespace ZumZumFood.Domain.Entities
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OrderId")]
        public int OrderId { get; set; }

        [Column("OrderFullName", TypeName = "nvarchar(200)")]
        public string OrderFullName { get; set; }

        [Column("OrderAddress", TypeName = "nvarchar(200)")]
        public string OrderAddress { get; set; }

        [Column("OrderPhoneNumber", TypeName = "nvarchar(15)")]
        [Phone]
        public string OrderPhoneNumber { get; set; }

        [Column("OrderEmail", TypeName = "nvarchar(200)")]
        [EmailAddress]
        public string? OrderEmail { get; set; }

        [Column("OrderDate")]
        public DateTime? OrderDate { get; set; } = default(DateTime?);

        [Column("OrderPaymentMethods", TypeName = "nvarchar(100)")]
        public string? OrderPaymentMethods { get; set; }

        [Column("OrderStatusPayment", TypeName = "nvarchar(100)")]
        public string? OrderStatusPayment { get; set; }

        [Column("OrderStatus")]
        public int? OrderStatus { get; set; }
        public int? OrderQuantity { get; set; }

        [Column("OrderAmount")]
        [Range(0, double.MaxValue)]
        public double? OrderAmount { get; set; }

        [Column("OrderNote", TypeName = "ntext")]
        public string? OrderNote { get; set; }

        [Column]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual User User { get; set; }

        [JsonIgnore]
        public virtual ICollection<CouponOrder> CouponOrders { get; set; } = new List<CouponOrder>();

        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    }
}
