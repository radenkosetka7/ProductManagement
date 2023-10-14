using ProductsManagement.Models.Enums;

namespace ProductsManagement.Models.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Unit Unit { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public User User { get; set; }
        public Category Category { get; set; }
        public ICollection<AttributeValue> AttributeValues { get; set; }
    }
}
