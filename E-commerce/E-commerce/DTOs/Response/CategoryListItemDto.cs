namespace E_commerce.DTOs.Response
{
    public class CategoryListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}
