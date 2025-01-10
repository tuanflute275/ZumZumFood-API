namespace ZumZumFood.Application.Models.Request
{
    public class OrderModel
    {
        public int UserId { get; set; }
        public string? OrderFullName { get; set; }
        public string? OrderAddress { get; set; }
        public string? OrderPhoneNumber { get; set; }
        public string? OrderEmail { get; set; }
        public string? OrderPaymentMethods { get; set; }
        public string? OrderStatusPayment { get; set; }
        public int? OrderQuantity { get; set; }
        public double? OrderAmount { get; set; }
        public string? OrderNote { get; set; }
    }
}
