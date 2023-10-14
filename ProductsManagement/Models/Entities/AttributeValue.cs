namespace ProductsManagement.Models.Entities
{
    public class AttributeValue
    {
        public Guid ProductId { get; set; }
        public Guid AttributeId { get; set; }

        public string Value { get; set; }
        public Product Product { get; set; }
        public Attribute Attribute { get; set; }
    }
}
