using ProductsManagement.Models.Entities;

namespace ProductsManagement.Models.DTOs
{
    public class AttributeValueDTO
    {
        public string Value { get; set; }
        public AttributeDTO Attribute { get; set; }
    }
}
