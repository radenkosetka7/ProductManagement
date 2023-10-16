using ProductsManagement.Models.DTOs;
using ProductsManagement.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagement.Models.Requests
{
    public class FilterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string CategoryName { get; set;} = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public ICollection<AttributeValueDTO> AttributeValues { get; set; } = new List<AttributeValueDTO>();
    }
}
