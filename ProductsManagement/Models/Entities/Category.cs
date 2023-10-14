namespace ProductsManagement.Models.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; } = new List<Product>();
    }
}
