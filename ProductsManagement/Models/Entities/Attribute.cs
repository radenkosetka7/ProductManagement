using ProductsManagement.Models.Enums;

namespace ProductsManagement.Models.Entities
{
    public class Attribute
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AttributeType Type { get; set; }
        public ICollection<AttributeValue> AttributeValues { get; set; }
    }
}
