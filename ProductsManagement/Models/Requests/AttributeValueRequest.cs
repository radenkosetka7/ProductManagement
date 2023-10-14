using System.ComponentModel.DataAnnotations;

namespace ProductsManagement.Models.Requests
{
    public class AttributeValueRequest
    {
        [Required(ErrorMessage = "Value is required!")]
        public string Value { get; set; }
        [Required(ErrorMessage = "Attribute is required!")]
        public Guid AttributeId { get; set; }

    }
}
