namespace E_commerce.DTOs.Response
{
    public class GetCategoriesResponseDto : SpResponseDto
    {
        public List<CategoryListItemDto> Categories { get; set; } = [];
    }
}

