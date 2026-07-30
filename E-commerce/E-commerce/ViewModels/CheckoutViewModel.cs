using E_commerce.DTOs;
using E_commerce.Enums;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.ViewModels
{
    public class CheckoutViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^(03\d{9}|\+923\d{9})$", ErrorMessage = "Enter a valid Pakistani mobile number.")]
        public string PhoneNumber { get; set; } = string.Empty;
        public int PaymentMethodId { get; set; } = (int)PaymentMethod.CashOnDelivery;
        public List<CartItemDto> Items { get; set; } = [];
        public decimal Total => Items.Sum(i => i.Subtotal);
    }
}
