namespace E_commerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
