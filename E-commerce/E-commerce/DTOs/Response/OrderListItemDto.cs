namespace E_commerce.DTOs.Response
{
    public class OrderListItemDto
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }
        public int PaymentMethodId { get; set; }
        public int ItemCount { get; set; }
        public decimal Total { get; set; }
    }
}
