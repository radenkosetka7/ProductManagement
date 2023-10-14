using ProductsManagement.Models.Entities;
using ProductsManagement.Models.Enums;

namespace ProductsManagement.Models.DTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public CategoryDTO Category { get; set; }
        public ICollection<AttributeValueDTO> AttributeValues { get; set; }
    }
}
