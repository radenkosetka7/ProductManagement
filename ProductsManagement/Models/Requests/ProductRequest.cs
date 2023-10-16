using ProductsManagement.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagement.Models.Requests
{
    public class ProductRequest
    {
        [Required(ErrorMessage = "Code is required!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Unit is required!")]
        public Unit Unit { get; set; }
        [Required(ErrorMessage = "Category is required!")]
        public Guid CategoryId { get; set; }
        [Required(ErrorMessage = "Attributes are required!")]
        public ICollection<AttributeValueRequest> AttributeValues { get; set; }
    }
}
