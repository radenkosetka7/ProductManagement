using ProductsManagement.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagement.Models.Requests
{
    public class AttributeRequest
    {
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Type is required!")]
        public AttributeType Type { get; set; }
    }
}
