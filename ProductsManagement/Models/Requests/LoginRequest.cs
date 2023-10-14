using System.ComponentModel.DataAnnotations;

namespace ProductsManagement.Models.Requests
{
    public class LoginRequest
    {
        [EmailAddress(ErrorMessage = "Please insert valid e-mail address!")]
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
