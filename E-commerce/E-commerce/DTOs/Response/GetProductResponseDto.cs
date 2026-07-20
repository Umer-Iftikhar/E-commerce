namespace E_commerce.DTOs.Response
{
    public class GetProductResponseDto : ApiResponseDto
    {
        public ProductDetailDto? Product { get; set; }
    }
}
