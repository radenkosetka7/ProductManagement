namespace ProductsManagement.Models.Entities
{
    public class AttributeValue
    {
        public Guid ProductsId { get; set; }
        public Guid AttributesId { get; set; }

        public string Value { get; set; }
        public Product Product { get; set; }
        public Attribute Attribute { get; set; }
    }
}
