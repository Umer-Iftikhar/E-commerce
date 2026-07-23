using E_commerce.CustomAttributes;
using E_commerce.DTOs;
using E_commerce.DTOs.Response;

namespace E_commerce.ViewModels
{
    public class ProductIndexViewModel
    {
        public List<ProductListItemDto> Products { get; set; } = [];
        public List<CategoryListItemDto> Categories { get; set; } = [];

        [DateRange]
        public DateOnly? CreatedDate { get; set; }
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
    }
}
