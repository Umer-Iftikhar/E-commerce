using System.ComponentModel.DataAnnotations;

namespace E_commerce.Enums
{
    public enum OrderStatus 
    {
        [Display(Name = "Pending")]
        Pending = 1,
        [Display(Name = "Processing")]
        Processing = 2,
        [Display(Name = "Shipped")]
        Shipped = 3,
        [Display(Name = "Delivered")]
        Delivered = 4,
        [Display(Name = "Cancelled")]
        Cancelled = 5
    }
}
