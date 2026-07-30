namespace E_commerce.DTOs.Request
{
    public class CreateOrderRequestDto
    {
        public int UserId { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int PaymentMethodId { get; set; }
    }
}
