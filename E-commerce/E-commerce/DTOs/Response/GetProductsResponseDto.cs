namespace E_commerce.DTOs.Response
{
    public class GetProductsResponseDto : SpResponseDto
    {
        public List<ProductListItemDto> Products { get; set; } = [];
    }
}
