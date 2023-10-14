using System.ComponentModel.DataAnnotations;

namespace ProductsManagement.Models.Requests
{
    public class CategoryRequest
    {
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
    }
}
