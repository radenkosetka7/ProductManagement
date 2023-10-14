using ProductsManagement.Models.Enums;

namespace ProductsManagement.Models.Requests
{
    public class ProductRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Unit Unit { get; set; }
        public Guid CategoryId { get; set; }
        public ICollection<AttributeValueRequest> AttributeValues { get; set; }
    }
}
