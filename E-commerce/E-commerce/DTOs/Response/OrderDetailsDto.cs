namespace E_commerce.DTOs.Response
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }
        public int PaymentMethodId { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}
